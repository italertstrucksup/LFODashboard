using DataAccessInterface;
using ManageAccessService.BL.Implementation;
using ManageAccessService.BL.Interface;
using ManageAccessService.DAL.Implementation;
using ManageAccessService.DAL.Interface;
using Scalar.AspNetCore;
using System.Runtime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IAccessBL, AccessBL>();
builder.Services.AddScoped<IAccessDAL, AccessDAL>();
// 🔥 ADD THIS (Missing)
builder.Services.AddScoped<IDataAccess, SqlDataAccess>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

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
