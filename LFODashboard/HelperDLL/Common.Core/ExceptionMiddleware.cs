using Common.Core;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Common.Core {

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            int statusCode;
            string message;

            switch (ex)
            {
                case BadRequestException badRequest:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = badRequest.Message;
                    break;

                case UnauthorizedException unauthorized:
                    statusCode = StatusCodes.Status401Unauthorized;
                    message = unauthorized.Message;
                    break;

                case NotFoundException notFound:
                    statusCode = StatusCodes.Status404NotFound;
                    message = notFound.Message;
                    break;

                case AppException appEx:
                    statusCode = appEx.StatusCode;
                    message = appEx.Message;
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "Internal Server Error";
                    break;
            }

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = message,
                StatusCode = statusCode,
                Data = null
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(json);
        }
    }


}
