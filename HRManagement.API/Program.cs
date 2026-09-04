using HRManagement.API.Services;
using HRManagement.Application.Common.Interfaces;
using Microsoft.OpenApi;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console().CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found");

builder.Services.AddInfrastructure(connectionString, builder.Configuration);
builder.Services.AddSerilog(
    (services, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(builder.Configuration).ReadFrom.Services(services));

builder.Services
  .AddAuthentication(options =>
  {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  })
  .AddJwtBearer(options =>
  {
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = jwtSettings!.Issuer,
          ValidAudience = jwtSettings.Audience,
          IssuerSigningKey = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
          ClockSkew = TimeSpan.Zero
      };
  });
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddApplication();
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
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter jwt token only. Example: eyjdfddfd..."
    });

    options.AddSecurityRequirement(d => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", d)] = new List<string>()
    });
});

var app = builder.Build();

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate =
    "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.000} ms";

    options.GetLevel = (httpContext, _, exception) =>
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
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
