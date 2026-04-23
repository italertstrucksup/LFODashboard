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
builder.Services.AddScoped<IDLService, DLService>();
builder.Services.AddScoped<IDLDAL, DLDAL>();
builder.Services.AddScoped<IPanService,PanService>();
builder.Services.AddScoped<IPanDal,PanDAL>();
builder.Services.AddScoped<IVoterService,VoterService>();
builder.Services.AddScoped<IVoterDAL,VoterDAL>();
builder.Services.AddScoped<IRCDetailsService, RCDetailService>();
builder.Services.AddScoped<IRCDetailsDAL,RCDetailsDAL>();
builder.Services.AddScoped<CL_JWTtokenGeneration>();

builder.Services.AddHttpClient<IHttpService, HttpService>();




var app = builder.Build();
app.UseMiddleware<Common.Core.ExceptionMiddleware>();
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
