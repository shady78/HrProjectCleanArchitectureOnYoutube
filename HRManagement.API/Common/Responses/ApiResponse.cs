namespace HRManagement.API.Common.Responses
{
    public sealed record ApiResponse<T>
    (bool Success ,string Message,T? Data , object? Errors)
    {
        public static ApiResponse<T> Succeeded(
            T data, string message)
        {
            return new ApiResponse<T>(
                true, message, data, null);
        }

        public static ApiResponse<T> Failed(
            string message, object? errors = null)
        {
            return new ApiResponse<T>(
                false, message, default, errors);
        }
    }
}
