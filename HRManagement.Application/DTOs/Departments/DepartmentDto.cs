namespace HRManagement.Application.DTOs.Departments
{
    public record CreateDepartmentRequest(string Name, string? Locaiton);

    public record UpdateDepartmentRequest(string Name, string? Locaiton);

    public record DepartmentResponse(int Id, string Name, string? Locaiton, bool IsActive);
}
