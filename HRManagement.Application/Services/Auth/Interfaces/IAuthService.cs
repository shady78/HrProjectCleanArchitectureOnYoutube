using HRManagement.Application.DTOs.Auth;

namespace HRManagement.Application.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(
            RegisterRequest request, CancellationToken cancellationToken = default);

        Task<AuthResponse> LoginAsync(
            LoginRequest request, CancellationToken cancellationToken = default);

        Task<AuthResponse> RefreshTokenAsync(
            RefreshTokenRequest request, CancellationToken cancellationToken = default);

        Task RevokeRefreshTokenAsync(
            RevokeRefreshTokenRequest request, CancellationToken cancellationToken = default);
    }
}
