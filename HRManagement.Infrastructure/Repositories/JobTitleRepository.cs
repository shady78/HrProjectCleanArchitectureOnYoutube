namespace HRManagement.Infrastructure.Repositories
{
    public class JobTitleRepository(ApplicationDbContext _context) : IJobTitleRepository
    {
        public async Task<IReadOnlyList<JobTitle>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.JobTitles
                .AsNoTracking()
                .OrderBy(j => j.Title)
                .ToListAsync(cancellationToken);
        }
        public async Task<JobTitle?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.JobTitles.FirstOrDefaultAsync(j => j.Id == id,cancellationToken);
        }
        public Task<bool> TitleExistsAsync(string title, CancellationToken cancellationToken = default)
        {
            return _context.JobTitles.AnyAsync(
                j => j.Title == title, cancellationToken);
        }
        public async Task AddAsync(JobTitle jobTitle, CancellationToken cancellationToken = default)

        {
            await _context.JobTitles.AddAsync(jobTitle, cancellationToken);
        }


        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
