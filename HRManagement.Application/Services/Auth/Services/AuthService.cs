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
        public AuthService(UserManager<ApplicaitonUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResponse> RegisterAsync(
            RegisterRequest request,
            CancellationToken cancellationToken = default)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
            {
                throw new Exception($"User with email {request.Email} already exists.");
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

            var tokenResult = await _tokenService.GenerateAccessTokenAsync(user, cancellationToken);

            return new AuthResponse
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                AccessToken = tokenResult.Token,
                AccessTokenExpiresAt = tokenResult.ExpiresAt
            };
        }
        public async Task<AuthResponse> LoginAsync(
            LoginRequest request,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                throw new Exception("Invalid email or password.");
            }
            if (!user.IsActive)
            {
                throw new Exception("User is not active.");
            }
            var passwordValid = await _userManager.CheckPasswordAsync(
                user, request.Password);
            if (!passwordValid)
            {
                throw new Exception("Invalid email or password.");
            }
            var tokenResult = await _tokenService.GenerateAccessTokenAsync(user, cancellationToken);

            return new AuthResponse
            {
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email!,
                AccessToken = tokenResult.Token,
                AccessTokenExpiresAt = tokenResult.ExpiresAt
            };
        }

    }
}
