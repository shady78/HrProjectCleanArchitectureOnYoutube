namespace HRManagement.API.Filters
{
    public sealed class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var failuers = new List<ValidationFailure>();

            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument is null)
                {
                    continue;
                }
                var validatorType = typeof(IValidator<>)
                    .MakeGenericType(argument.GetType());

                var validator = context.HttpContext.RequestServices
                    .GetService(validatorType) as IValidator;

                if (validator is null)
                {
                    continue;
                }
                var validationContext = new ValidationContext<object>(argument);

                var validationResult = await validator.ValidateAsync(
                    validationContext, context.HttpContext.RequestAborted);

                failuers.AddRange(validationResult.Errors);
            }

            if (failuers.Count == 0)
            {
                await next();
                return;
            }

            var errors = failuers
                .GroupBy(error => error.PropertyName)
                .Select(group => new ValidationError(
                    group.Key,
                    group.Select(error => error.ErrorMessage)
                    .Distinct().ToArray())).ToArray();

            context.Result = new BadRequestObjectResult(
                ApiResponse<object?>.Failed("Validation failed.", errors));
        }
    }
}
