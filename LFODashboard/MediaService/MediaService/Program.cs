using Amazon.S3;
using MediaService.BL;
using MediaService.BL.IBussinessLayer;
using MediaService.BL.Model;
using MediaServiceAPI.BussinessLayer;
using Microsoft.AspNetCore.Mvc;
using System.Runtime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    options.CustomSchemaIds(x => x.FullName);
});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

// Configure S3 Settings from appsettings.json
var s3Settings = builder.Configuration.GetSection("AWS").Get<S3Settings>();
builder.Services.Configure<S3Settings>(builder.Configuration.GetSection("AWS"));

// Register AWS S3 Client as a Singleton
if (s3Settings != null && !string.IsNullOrEmpty(s3Settings.AwsAccessKeyId) && !string.IsNullOrEmpty(s3Settings.AwsSecretKey))
{
    builder.Services.AddSingleton<IAmazonS3>(new AmazonS3Client(s3Settings.AwsAccessKeyId, s3Settings.AwsSecretKey, Amazon.RegionEndpoint.APSouth1));
}
else
{
    builder.Services.AddSingleton<IAmazonS3>(new AmazonS3Client(Amazon.RegionEndpoint.APSouth1));
}

// Dependency Injection registration
builder.Services.AddScoped<IMediaBusinessLayer, MediaBusinessLayer>();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature?.Error != null)
        {
            await context.Response.WriteAsJsonAsync(new
            {
                error = exceptionHandlerPathFeature.Error.Message,
                detail = exceptionHandlerPathFeature.Error.StackTrace
            });
        }
    });
});

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
