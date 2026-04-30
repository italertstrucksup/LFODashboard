using Asp.Versioning;
using DashboardService.BL.Implementation;
using DashboardService.BL.Interface;
using DashboardService.DAL.Implementation;
using DashboardService.DAL.Interface;
using DataAccessInterface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddScoped<IDataAccess, SqlDataAccess>();
builder.Services.AddScoped<IDashboardDAL, DashboardDAL>();
builder.Services.AddScoped<IDashboardBL, DashboardBL>();


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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
