using AuthServices_LFO.BL.Implemetation;
using AuthServices_LFO.BL.Interface;
using AuthServices_LFO.DAL.Implemetation;
using AuthServices_LFO.DAL.Interface;
using DataAccessInterface;
using HttpClientLib;
using JwtAuthenticationManager;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IDataAccess, SqlDataAccess>();
builder.Services.AddHttpClient<IHttpService, HttpService>();
builder.Services.AddScoped<IAuthBL, AuthBL>();
builder.Services.AddScoped<IAuthDAL, AuthDAL>();
builder.Services.AddControllers();
builder.Services.AddCustomJwtAuthentication(builder.Configuration);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
