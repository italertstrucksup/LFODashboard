using System.Runtime;
using MediaService.BL;
using MediaService.BL.IBussinessLayer;
using MediaService.BL.Model;
using MediaServiceAPI.BussinessLayer;

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

// Configure S3 Settings from appsettings.json
builder.Services.Configure<S3Settings>(builder.Configuration.GetSection("AWS"));

// Dependency Injection registration
builder.Services.AddScoped<IMediaBusinessLayer,MediaBusinessLayer>();

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
