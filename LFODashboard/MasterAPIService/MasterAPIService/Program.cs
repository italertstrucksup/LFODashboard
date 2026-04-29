using DataAccessInterface;
using HttpClientLib;
using MasterAPIServiceBL.Implementation;
using MasterAPIServiceBL.Interface;
using MasterAPIServiceDAL.Implmentation;
using MasterAPIServiceDAL.Interface;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IDataAccess, SqlDataAccess>();
builder.Services.AddHttpClient<IHttpService, HttpService>();
builder.Services.AddScoped<IMasterBL, MasterBL>();
builder.Services.AddScoped<IMasterDAL, MasterDAL>();
builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
