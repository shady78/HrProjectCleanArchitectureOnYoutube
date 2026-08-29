using HRManagement.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace HRManagement.Infrastructure.Persistence
{
    public sealed class AuditSaveChangesInterceptor
        (ICurrentUserService currentUserService) : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges
            (DbContextEventData eventData, InterceptionResult<int> result)
        {
            ApplyAuditRules(eventData.Context);
            return base.SavingChanges(eventData, result);
        }
        
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync
            (DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            ApplyAuditRules(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        private void ApplyAuditRules(DbContext? context)
        {
            if (context is null)
            {
                return;
            }
            var utcNow = DateTime.UtcNow;
            var currentUser = currentUserService.GetCurrentUserId();

            SetAuditFields(context, currentUser, utcNow);
            var auditLogs = BuildAuditLogs(context, currentUser, utcNow);
            if (auditLogs.Count > 0)
            {
                context.Set<AuditLog>().AddRange(auditLogs);
            }
        }

        private static void SetAuditFields(
            DbContext context,
            string currentUser,
            DateTime utcNow)
        {
            foreach (var entry in context.ChangeTracker.Entries<BaseAuditEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = utcNow;
                    entry.Entity.CreatedBy = currentUser;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = utcNow;
                    entry.Entity.UpdatedBy = currentUser;
                }
                if (entry.State == EntityState.Deleted)
                {
                    // .Remove , Delete * from tableName where id = Id
                    // Update set IsDeleted = true
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedBy = currentUser;
                    entry.Entity.DeletedAt = utcNow;
                }
            }
        }

        private static List<AuditLog> BuildAuditLogs
            (DbContext context,
            string currentUser,
            DateTime utcNow)
        {
            var logs = new List<AuditLog>();
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog)
                {
                    continue;
                }
                // Only log Added, Modified, Deleted entities  , Unchanged , Detached
                if (entry.State is not (EntityState.Added or
                        EntityState.Modified or
                        EntityState.Deleted))
                {
                    continue;
                }
                var changes = BuildChangesDictionary(entry);
                if (changes.Count == 0)
                {
                    continue;
                }
                logs.Add(new AuditLog
                {
                    EntityName = entry.Entity.GetType().Name,
                    EntityId = entry.State == EntityState.Added
                        ? string.Empty
                        : GetPrimaryKeyValue(entry),
                    Action = entry.State.ToString(),
                    ChangedAt = utcNow,
                    ChangedBy = currentUser,
                    Changes = JsonSerializer.Serialize(changes)
                });
            }
            return logs;
        }
        private static Dictionary<string, object?> BuildChangesDictionary(
            EntityEntry entry)
        {
            var changes = new Dictionary<string, object?>();
            foreach (var property in entry.Properties)
            {
                switch (entry.State)
                {
                    case EntityState.Added when !property.IsTemporary:
                        changes[property.Metadata.Name] = property.CurrentValue;
                        break;
                    case EntityState.Deleted:
                        changes[property.Metadata.Name] = property.OriginalValue;
                        break;

                    case EntityState.Modified when property.IsModified:
                        changes[property.Metadata.Name] = new
                        {
                            OldValue = property.OriginalValue,
                            NewValue = property.CurrentValue
                        };
                        break;
                }
            }
            return changes;
        }

        private static string? GetPrimaryKeyValue(EntityEntry entry)
        {
            var key = entry.Metadata.FindPrimaryKey();

            if (key is null)
            {
                return string.Empty;
            }
            var values = key.Properties
                .Select(prop => entry.Property(prop.Name)
                .CurrentValue?.ToString()?? string.Empty)
                .Where(value => value is not null);

            return string.Join(",", values);
        }
    }
}
