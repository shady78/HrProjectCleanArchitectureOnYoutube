using HRManagement.Domain.Entities;
using HRManagement.Domain.Entities.Identity;

namespace HRManagement.Application.Services.Auth.Interfaces
{
    public interface ITokenService
    {
        Task<(string Token, DateTime ExpiresAt)> GenerateAccessTokenAsync(
            ApplicaitonUser user, CancellationToken cancellationToken = default);

        RefreshToken GetRefreshToken();
    }
}
