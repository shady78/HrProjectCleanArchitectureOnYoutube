using Serilog;
using Serilog.Events;
using Serilog.Extensions.Hosting;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console().CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog (
    (services, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(builder.Configuration).ReadFrom.Services(services));

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found");
 
builder.Services.AddApplication();
builder.Services.AddInfrastructure(connectionString);
builder.Services.AddScoped<ValidationFilter>();
builder.Services.AddControllers(options =>
{
    options.Filters.AddService<ValidationFilter>();
})
 .ConfigureApiBehaviorOptions(options =>
 {
     options.InvalidModelStateResponseFactory = context =>
     {
         var errors = context.ModelState
         .Where(item => item.Value?.Errors.Count > 0)
         .Select(item => new ValidationError(
             item.Key,
             item.Value!.Errors.
             Select(error => string.IsNullOrWhiteSpace(error.ErrorMessage)
             ? "the supplied value is valid." :
             error.ErrorMessage)
             .Distinct().ToArray())).ToArray();

         return new BadRequestObjectResult(
             ApiResponse<object?>.Failed("Validation Failed.", errors));
     };
 });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
var app = builder.Build();

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate =
    "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.000} ms";

    options.GetLevel =  (httpContext , _,exception)=>
        exception is not null || 
        httpContext.Response.StatusCode >= StatusCodes.Status500InternalServerError
        ? LogEventLevel.Error 
        : httpContext.Response.StatusCode >= StatusCodes.Status400BadRequest 
        ? LogEventLevel.Warning : LogEventLevel.Information;

    options.EnrichDiagnosticContext =
    (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set(
            "RequestHost",
            httpContext.Request.Host.Value);
        diagnosticContext.Set(
            "RequestScheme",
            httpContext.Request.Scheme);
    };
});

app.UseExceptionHandler();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "HR Management API v1");
        options.DocumentTitle = "HR Management API";
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
