namespace HRManagement.Domain.Entities
{
    public sealed class Department : BaseAuditEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Location { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
