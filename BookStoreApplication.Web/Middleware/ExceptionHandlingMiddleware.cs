using System.Text.Json;
using BookStoreApplication.Web.DTOs.ReviewsAndRatings;
using BookStoreApplication.Web.Exceptions;

namespace BookStoreApplication.Web.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch (RequestValidationException ex)
            {
                await WriteResponseAsync(context, ex.StatusCode, ex.Message, ex.Errors);
            }
            catch (ApiException ex)
            {
                await WriteResponseAsync(context, ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");
                await WriteResponseAsync(context, StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        private static async Task WriteResponseAsync(HttpContext context, int statusCode, string message, Dictionary<string, string[]>? errors = null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new ApiErrorResponseDto
            {
                StatusCode = statusCode,
                Message = message,
                Errors = errors
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
