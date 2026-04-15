using HttpClientLib;
using Scalar.AspNetCore;
using VendorService.BL.Implementation;
using VendorService.BL.Interface;
using VendorService.DAL.Interface;
using VendorService.DAL.Repository;
using Common.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register Wocom Services
builder.Services.AddScoped<IWocomBL,WocomBL>();
builder.Services.AddScoped<IWocomDAL,WocomDAL>();
builder.Services.AddScoped<IAadharService,AadharService>();
builder.Services.AddScoped<IAadhaarDAL,AadhaarDAL>();
builder.Services.AddScoped<CL_JWTtokenGeneration>();
builder.Services.AddHttpClient<IHttpService, HttpService>();



var app = builder.Build();
app.UseMiddleware<Common.Core.AppException>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
