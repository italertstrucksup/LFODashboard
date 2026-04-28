using DataAccessInterface;
using HttpClientLib;
using ProfilePhotoService.BL.Implemetation;
using ProfilePhotoService.BL.Interface;
using ProfilePhotoService.DAL.Implemetation;
using ProfilePhotoService.DAL.Interface;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// DI
builder.Services.AddScoped<IDataAccess, SqlDataAccess>();
builder.Services.AddScoped<IProfilePhotoBL, ProfilePhotoBL>();
builder.Services.AddScoped<IProfilePhotoDAL, ProfilePhotoDAL>();
builder.Services.AddHttpClient<IHttpService, HttpService>()
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();