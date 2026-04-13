using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register Wocom Services
builder.Services.AddScoped<VendorService.BL.Interface.IWocomBL, VendorService.BL.Implementation.WocomBL>();
builder.Services.AddScoped<VendorService.DAL.Interface.IWocomDAL, VendorService.DAL.Repository.WocomDAL>();

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
