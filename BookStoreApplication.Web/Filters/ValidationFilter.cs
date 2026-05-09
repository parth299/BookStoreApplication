using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BookStoreApplication.Web.Wrappers;
using FluentValidation;

namespace BookStoreApplication.Web.Filters
{
    /// <summary>
    /// Custom Validation Filter that validates ModelState before action execution
    /// Returns 400 BadRequest with validation errors if ModelState is invalid
    /// Integrates with FluentValidation for detailed error messages
    /// </summary>
    public class ValidationFilter : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ValidationFilter> _logger;

        public ValidationFilter(IServiceProvider serviceProvider, ILogger<ValidationFilter> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Check if ModelState is valid (includes FluentValidation results when configured)
            if (!context.ModelState.IsValid)
            {
                // Extract all validation errors from ModelState
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors.Select(e => $"{x.Key}: {e.ErrorMessage}"))
                    .ToList();

                _logger.LogWarning(
                    "[ValidationFilter] Validation failed for {Controller}.{Action}: {Errors}",
                    context.Controller.GetType().Name,
                    context.ActionDescriptor.DisplayName,
                    string.Join("; ", errors));

                // Return standardized error response
                var response = ApiResponse.Fail<object>(
                    "Validation failed. Please check your input.",
                    errors);

                context.Result = new BadRequestObjectResult(response);
                return;
            }

            // If valid, continue to action execution
            await next();
        }
    }
}
