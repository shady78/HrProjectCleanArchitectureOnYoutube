using HRManagement.Infrastructure;
using HRManagement.Application;
using HRManagement.API.Filters;
var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
