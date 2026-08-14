using HRManagement.Application.DTOs.Departments;
using HRManagement.Domain.Entities;

namespace HRManagement.Application.Mappings
{
    public static class DepartmentMapping
    {
        public static Department ToEntity(this CreateDepartmentRequest request)
        {
            return new Department
            {
                Name = request.Name,
                Location = request.Locaiton
            };
        }

        public static void MapTo(this UpdateDepartmentRequest request, Department department)
        {
            department.Name = request.Name.Trim();
            department.Location = NormalizeLocation(request.Locaiton);
        }


        public static DepartmentResponse ToResponse(this Department department)
        {
            return new DepartmentResponse
                (
                    department.Id,
                    department.Name,
                    department.Location,
                    department.IsActive
                );
        }

        private static string? NormalizeLocation(string? location)
        {
            return string.IsNullOrWhiteSpace(location)
                ? null : location.Trim();
        }
    }
}
