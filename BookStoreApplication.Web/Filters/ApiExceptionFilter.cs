using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Wrappers;

namespace BookStoreApplication.Web.Filters
{
    /// <summary>
    /// Exception Filter that handles exceptions at the action/controller level
    /// Catches exceptions and converts them to standardized API responses
    /// Acts as local exception handling before global middleware kicks in
    /// </summary>
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var actionName = context.ActionDescriptor.DisplayName;
            
            _logger.LogError(
                exception,
                "[ApiExceptionFilter] Exception in {Action}: {Message}",
                actionName,
                exception.Message);

            // Determine status code based on exception type
            int statusCode = exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            // Create standardized error response
            var response = ApiResponse.Fail<object>(
                exception.Message,
                exception is BaseException ? null : new List<string> { exception.StackTrace ?? "No stack trace available" });

            context.Result = new ObjectResult(response)
            {
                StatusCode = statusCode
            };
            
            // Mark exception as handled to prevent global middleware from re-handling
            context.ExceptionHandled = true;
        }
    }
}
