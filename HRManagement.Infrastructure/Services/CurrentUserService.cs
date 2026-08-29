using HRManagement.Application.Common.Interfaces;

namespace HRManagement.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string GetCurrentUserId() => "System";
    }
}
