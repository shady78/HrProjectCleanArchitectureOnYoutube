using HRManagement.Application.Repositories;
using HRManagement.Domain.Entities;
using HRManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRManagement.Infrastructure.Repositories
{
    public class DepartmentRepository(ApplicationDbContext _context) : IDepartmentRepository
    {

        public async Task AddAsync(Department department, CancellationToken cancellation = default)
        {
            await _context.Departments.AddAsync(department, cancellation);    
        }

        public async Task<IReadOnlyList<Department>> GetAllAsync(CancellationToken cancellation = default)
        {
            return await _context.Departments
                .AsNoTracking()
                .OrderBy(department => department.Name)
                .ToListAsync(cancellation);
        }

        public async Task<Department> GetById(int id, bool trackchanges = false, CancellationToken cancellation = default)
        {
            IQueryable<Department> query = _context.Departments;

            if (!trackchanges)
            {
                query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(
                d => d.Id == id, cancellation);
        }

        public Task<bool> NameExistsAsync(string name, CancellationToken cancellation = default)
        {
            return _context.Departments.AnyAsync(
                d => d.Name == name, cancellation);
        }

        public Task<int> SaveChangeAsync(CancellationToken cancellation = default)
        {
            return _context.SaveChangesAsync(cancellation);
        }
    }
}
