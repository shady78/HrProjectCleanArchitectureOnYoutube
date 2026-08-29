namespace HRManagement.Domain.Entities
{
    public sealed class JobTitle : BaseAuditEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
