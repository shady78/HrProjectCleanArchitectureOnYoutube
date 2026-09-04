using HRManagement.Application.Common.Exceptions;
using HRManagement.Application.Common.Interfaces;
using HRManagement.Application.DTOs.Auth;
using HRManagement.Application.Services.Auth.Interfaces;
using HRManagement.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace HRManagement.Application.Services.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicaitonUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        public AuthService(UserManager<ApplicaitonUser> userManager, ITokenService tokenService, IUserRepository userRepository)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        public async Task<AuthResponse> RegisterAsync(
            RegisterRequest request,
            CancellationToken cancellationToken = default)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                throw new UnauthorizedException($"User with email {request.Email} already exists.");
            }
            var user = new ApplicaitonUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                IsActive = true
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"User registration failed: {errors}");
            }

            return await CreateAuthResponseAsync(user, cancellationToken);
        }
        public async Task<AuthResponse> LoginAsync(
            LoginRequest request,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }
            if (!user.IsActive)
            {
                throw new UnauthorizedException("User is not active.");
            }
            var passwordValid = await _userManager.CheckPasswordAsync(
                user, request.Password);
            if (!passwordValid)
            {
                throw new UnauthorizedException("Invalid email or password.");
            }
            return await CreateAuthResponseAsync(user, cancellationToken);
        }

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);
            if (user is null)
            {
                throw new UnauthorizedException("Invalid refresh token.");
            }
            var oldRefreshToken = user.RefreshTokens
                .FirstOrDefault(rt => rt.Token == request.RefreshToken);

            if (!oldRefreshToken!.IsActive)
            {
                throw new UnauthorizedException("Refresh token is expired or revoked ");
            }
            var newRefreshToken = _tokenService.GetRefreshToken();
            oldRefreshToken.RevokedAt = DateTime.UtcNow;
            oldRefreshToken.ReplacedByToken = newRefreshToken.Token;

            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var (accessToken, accessTokenExpiresAt) =
                await _tokenService.GenerateAccessTokenAsync(user, cancellationToken);

            return new AuthResponse
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = $"{user.FirstName} {user.LastName}",
                AccessToken = accessToken,
                AccessTokenExpiresAt = accessTokenExpiresAt,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiresAt = newRefreshToken.ExpiresAt
            };
        }

        public async Task RevokeRefreshTokenAsync(RevokeRefreshTokenRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByRefreshTokenAsync(
                request.RefreshToken, cancellationToken);

            if (user is null)
            {
                throw new UnauthorizedException("Invalid Refresh token.");
            }
            var refreshToken = user.RefreshTokens
                .FirstOrDefault(rt => rt.Token == request.RefreshToken);

            if (!refreshToken!.IsActive)
            {
                return;
            }
            refreshToken.RevokedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        // helper method 
        private async Task<AuthResponse> CreateAuthResponseAsync(
            ApplicaitonUser user, CancellationToken cancellationToken)
        {
            var tokenResult = await _tokenService.GenerateAccessTokenAsync(user, cancellationToken);
            var refreshToken = _tokenService.GetRefreshToken();
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);
            return new AuthResponse
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                FullName = $"{user.FirstName} {user.LastName}",
                AccessToken = tokenResult.Token,
                AccessTokenExpiresAt = tokenResult.ExpiresAt,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiresAt = refreshToken.ExpiresAt
            };
        }
    }
}
