using HRManagement.Application.DTOs.Auth;
using HRManagement.Application.Services.Auth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
        {
            var response = await _authService.RegisterAsync(request, cancellationToken);

            return Ok(ApiResponse<AuthResponse>.Succeeded(
                response,
                "User registered successfully."));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
        {
            var response = await _authService.LoginAsync(request, cancellationToken);
            return Ok(ApiResponse<AuthResponse>.Succeeded(
                response,
                "User logged in successfully."));
        }
    }
}
