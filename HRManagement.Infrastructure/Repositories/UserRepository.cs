using HRManagement.Application.Common.Interfaces;

namespace HRManagement.Infrastructure.Repositories
{
    public class UserRepository(ApplicationDbContext _context) : IUserRepository
    {
        public async Task<ApplicaitonUser?> GetByEmailWithRefreshTokensAsync(string email, CancellationToken cancellationToken = default)
        {
           
            return await _context.Users
                .Include(user => user.RefreshTokens)
                .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
        }

        public async Task<ApplicaitonUser?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .Include(user => user.RefreshTokens)
                .FirstOrDefaultAsync(user => user.RefreshTokens.Any(rt => rt.Token == refreshToken)
                , cancellationToken);
        }
    }
}
