namespace HRManagement.Domain.Entities
{
    public record PermissionDefinition(
        string Name,
        string Description,
        string Module
    );
}
