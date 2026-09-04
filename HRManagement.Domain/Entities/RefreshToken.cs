using HRManagement.Domain.Entities.Identity;

namespace HRManagement.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? ReplacedByToken { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicaitonUser User { get; set; } = default!;

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => RevokedAt is null && !IsExpired;
    }
}
