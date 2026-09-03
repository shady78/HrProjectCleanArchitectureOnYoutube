using Microsoft.AspNetCore.Identity;

namespace HRManagement.Domain.Entities.Identity
{
    public class ApplicaitonUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
