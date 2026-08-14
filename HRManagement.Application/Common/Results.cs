namespace HRManagement.Application.Common
{
    public enum ErrorType
    {
        NotFound,
        Conflict
    }
    public sealed record ServiceError
        (string Code, string Message, ErrorType Type);

    public sealed class Result<T>
    {
        private Result(T? value, ServiceError? error)
        {
            Value = value;
            Error = error;
        }

        public T? Value { get; }
        public ServiceError? Error { get; }

        public bool IsSuccess => Error is null;

        public static Result<T> Success(T value)
            => new(value, null);

        public static Result<T> Failure(ServiceError error)
            => new(default, error);
    }
}
