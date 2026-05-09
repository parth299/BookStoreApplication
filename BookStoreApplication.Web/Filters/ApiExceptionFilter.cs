using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BookStoreApplication.Web.Exceptions;
using BookStoreApplication.Web.Wrappers;

namespace BookStoreApplication.Web.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(
            ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(
            ExceptionContext context)
        {
            var exception = context.Exception;

            var actionName =
                context.ActionDescriptor.DisplayName;

            _logger.LogError(
                exception,
                "[ApiExceptionFilter] Exception in {Action}: {Message}",
                actionName,
                exception.Message);

            int statusCode = exception switch
            {
                NotFoundException =>
                    StatusCodes.Status404NotFound,

                BadRequestException =>
                    StatusCodes.Status400BadRequest,

                UnauthorizedException =>
                    StatusCodes.Status401Unauthorized,

                ArgumentException =>
                    StatusCodes.Status400BadRequest,

                _ =>
                    StatusCodes.Status500InternalServerError
            };

            var response =
                ApiResponse<object>.FailResponse(
                    exception.Message,
                    exception is BaseException
                        ? null
                        : new List<string>
                        {
                            exception.StackTrace
                            ?? "No stack trace available"
                        });

            context.Result = new ObjectResult(response)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}