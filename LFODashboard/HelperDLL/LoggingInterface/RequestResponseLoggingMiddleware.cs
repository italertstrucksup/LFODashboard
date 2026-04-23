using LoggingInterface.Interface;
using LoggingInterface.Model;
using Microsoft.AspNetCore.Http;
using System.Text;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IDBConnection db)
    {
        var start = DateTime.Now;

        context.Request.EnableBuffering();

        var requestBody = "";
        if (context.Request.ContentLength > 0)
        {
            using var reader = new StreamReader(
                context.Request.Body,
                Encoding.UTF8,
                leaveOpen: true);

            requestBody = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        if (context.Request.Path.StartsWithSegments("/login"))
        {
            requestBody = "Sensitive Data Hidden";
        }

        var originalBodyStream = context.Response.Body;

        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        var executionTime = (DateTime.Now - start).TotalMilliseconds;

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        await db.SaveLogAsync(new LogModel
        {
            Path = context.Request.Path,
            Method = context.Request.Method,
            RequestBody = requestBody,
            ResponseBody = responseText,
            StatusCode = context.Response.StatusCode,
            UserId = context.User?.FindFirst("id")?.Value,
            IPAddress = context.Connection.RemoteIpAddress?.ToString(),
            ExecutionTimeMs = (long)executionTime,
            CreatedAt = DateTime.Now
        });

        await responseBody.CopyToAsync(originalBodyStream);
    }
}