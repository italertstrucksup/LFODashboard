using JwtAuthenticationManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddJsonFile("Ocelot.json",optional:false,reloadOnChange:true);
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddCustomJwtAuthentication(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
// Ensure authentication runs before authorization
app.UseAuthentication();

// Global middleware to require authentication for all incoming requests to the gateway.
// Exempt the token issuing endpoint and OpenAPI/Swagger so clients can obtain tokens and view docs.
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? string.Empty;

    // Allow anonymous access to the authentication endpoint where clients obtain tokens
    if ((path.StartsWith("/send_signup_otp", StringComparison.OrdinalIgnoreCase)||path.StartsWith("/signup_user", StringComparison.OrdinalIgnoreCase)||path.StartsWith("/login", StringComparison.OrdinalIgnoreCase)||path.StartsWith("/refresh-token", StringComparison.OrdinalIgnoreCase)||path.StartsWith("/verifyOtp", StringComparison.OrdinalIgnoreCase)) &&
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

await app.UseOcelot(); 

app.MapControllers();

app.Run();
