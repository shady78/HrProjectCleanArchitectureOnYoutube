namespace HRManagement.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicaitonUser> _userManager;

        public TokenService(IOptions<JwtSettings> jwtSettings, UserManager<ApplicaitonUser> userManager)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
        }

        public async Task<(string Token, DateTime ExpiresAt)> GenerateAccessTokenAsync(
            ApplicaitonUser user,
            CancellationToken cancellationToken = default)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

            var claims = new List<Claim>
            {
                  new(JwtRegisteredClaimNames.Sub, user.Id),
                  new(JwtRegisteredClaimNames.Email, user.Email!),
                  new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new(ClaimTypes.NameIdentifier, user.Id),
                  new(ClaimTypes.Email, user.Email!),
                  new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}".Trim())
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var credentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return (tokenValue, expiresAt);
        }
    }
}
