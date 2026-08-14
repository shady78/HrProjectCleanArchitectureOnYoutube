using HRManagement.Application.Common;
using HRManagement.Application.DTOs.Departments;

namespace HRManagement.Application.Services.Departments
{
    public interface IDepartmentService
    { 
        Task<IReadOnlyList<DepartmentResponse>> GetAllAsync(
          CancellationToken cancellationToken = default);

        Task<Result<DepartmentResponse>> GetByIdAsync(
          int id,
          CancellationToken cancellationToken = default);

        Task<Result<DepartmentResponse>> CreateAsync(
          CreateDepartmentRequest request,
          CancellationToken cancellationToken = default);

        Task<Result<DepartmentResponse>> UpdateAsync(
          int id,
          UpdateDepartmentRequest request,
          CancellationToken cancellationToken = default);

        Task<Result<bool>> DeactivateAsync(
          int id,
          CancellationToken cancellationToken = default);
    }
}
