namespace HRManagement.Application.DTOs.Auth
{
    public class RevokeRefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
