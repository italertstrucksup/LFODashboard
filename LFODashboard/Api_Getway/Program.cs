using DataAccessInterface;
using JwtAuthenticationManager;
using LoggingInterface.Interface;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("Ocelot.json",optional:false,reloadOnChange:true);
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCustomJwtAuthentication(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IDBConnection, DBConnection>();
builder.Services.AddScoped<IDataAccess, SqlDataAccess>(); // your existing implementation
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll"); // 👈 ADD HERE

// Logging FIRST
app.UseMiddleware<RequestResponseLoggingMiddleware>();
// Ensure authentication runs before authorization
app.UseAuthentication();

// Global middleware to require authentication for all incoming requests to the gateway.
// Exempt the token issuing endpoint and OpenAPI/Swagger so clients can obtain tokens and view docs.
app.Use(async (context, next) =>
{

    var path = context.Request.Path.Value ?? string.Empty;

    // Allow anonymous access to the authentication endpoint where clients obtain tokens
    var allowedPaths = new[]
    {
        "/login",
        "/send-reset-otp",
        "/reset-password",
        "/send-login-otp",
        "/login-with-otp",
        "/refresh-token",
        "/send_signup_otp",
        "/verifyOtp",
        "/signup_user"
        
    };

    if ( path.StartsWith("/capture.html", StringComparison.OrdinalIgnoreCase) &&
        context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
    {
        await next();
        return;
    }

    if (allowedPaths.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)) &&
        context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
    {
        await next();
        return;
    }

    // Allow anonymous access to OpenAPI/Swagger UI
    if (path.StartsWith("/openapi", StringComparison.OrdinalIgnoreCase) ||  path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase))
    {
        await next();
        return;
    }

    var user = context.User;
    if (user?.Identity == null || !user.Identity.IsAuthenticated)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Unauthorized");
        return;
    }

    await next();
});

app.UseAuthorization();

app.MapControllers();

await app.UseOcelot(); 

app.Run();
