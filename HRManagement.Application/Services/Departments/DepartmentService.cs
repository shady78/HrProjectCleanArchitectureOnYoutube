namespace HRManagement.Application.Services.Departments
{
    public class DepartmentService(IDepartmentRepository repository) : IDepartmentService
    {
        public async Task<IReadOnlyList<DepartmentResponse>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var departments = await repository.GetAllAsync(cancellationToken);

            return departments.Select(department => department.ToResponse())
                .ToList();
        }
        public async Task<Result<DepartmentResponse>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var department = await repository.GetById(id);
            if (department is null)
            {
                return Result<DepartmentResponse>.Failure(DepartmentErrors.NotFound(id));
            }
            return Result<DepartmentResponse>.Success(department.ToResponse());
        }

        public async Task<Result<DepartmentResponse>> CreateAsync(CreateDepartmentRequest request, CancellationToken cancellationToken = default)
        {
            var normalizedName = request.Name.Trim();
            if (await repository.NameExistsAsync(normalizedName , cancellationToken))
            {
                return Result<DepartmentResponse>.Failure(DepartmentErrors.DuplicateName(normalizedName));
            }
            //var currentUserId = _currentUser.UserId;
            var department = request.ToEntity();
            //department.CreatedBy = currentUserId!;
            await repository.AddAsync(department,cancellationToken);
            await repository.SaveChangeAsync(cancellationToken);

            return Result<DepartmentResponse>.Success(department.ToResponse());
        }
        public async Task<Result<DepartmentResponse>> UpdateAsync(int id, UpdateDepartmentRequest request, CancellationToken cancellationToken = default)
        {
            var department = await repository.GetById(id, true,cancellationToken);
            if (department is null)
            {
                return Result<DepartmentResponse>.Failure(DepartmentErrors.NotFound(id));
            }

            var normalizedName = request.Name.Trim();
            if (await repository.NameExistsAsync(normalizedName,cancellationToken))
            {
                return Result<DepartmentResponse>.Failure(DepartmentErrors.DuplicateName(normalizedName));
            }

            request.MapTo(department);
            await repository.SaveChangeAsync(cancellationToken);

            return Result<DepartmentResponse>.Success(department.ToResponse());
        }

        public async Task<Result<bool>> DeactivateAsync(int id, CancellationToken cancellationToken = default)
        {
            var department = await repository.GetById(id, true, cancellationToken);
            if (department is null)
            {
                return Result<bool>.Failure(DepartmentErrors.NotFound(id));
            }
            if (!department.IsActive)
            {
                return Result<bool>.Success(true);
            }
            department.IsActive = false;
            await repository.SaveChangeAsync(cancellationToken);
             
            return Result<bool>.Success(true);
        }
    }
}
