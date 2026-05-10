using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Wrappers;
using System.Net;
using System.Text.Json;

namespace BookStoreApplication.Web.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;

            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception occurred");

                await HandleExceptionAsync(
                    context,
                    ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType =
                "application/json";

            int statusCode = exception switch
            {
                NotFoundException =>
                    (int)HttpStatusCode.NotFound,

                BadRequestException =>
                    (int)HttpStatusCode.BadRequest,

                UnauthorizedException =>
                    (int)HttpStatusCode.Unauthorized,

                _ =>
                    (int)HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = statusCode;

            ApiResponse<string> response =
                exception is BaseException
                    ? ApiResponse<string>.FailResponse(
                        exception.Message)
                    : ApiResponse<string>.FailResponse(
                        "An unexpected error occurred.");

            var json =
                JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}