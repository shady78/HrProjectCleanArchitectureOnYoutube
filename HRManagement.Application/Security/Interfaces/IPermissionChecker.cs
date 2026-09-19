namespace HRManagement.Application.Security.Interfaces
{
    public interface IPermissionChecker
    {
        Task<bool> HasPermissionAsync(
            string userId,
            string permission,
            CancellationToken cancellation = default);
    }
}
