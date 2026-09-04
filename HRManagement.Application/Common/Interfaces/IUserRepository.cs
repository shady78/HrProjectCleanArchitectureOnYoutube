using HRManagement.Domain.Entities.Identity;

namespace HRManagement.Application.Common.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicaitonUser?> GetByEmailWithRefreshTokensAsync(
            string email, 
            CancellationToken cancellationToken = default);

        Task<ApplicaitonUser?> GetByRefreshTokenAsync(
            string refreshToken,
            CancellationToken cancellationToken = default);
    }
}
