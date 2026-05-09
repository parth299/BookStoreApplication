using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace BookStoreApplication.Web.Filters
{
    /// <summary>
    /// Action Filter that logs the execution time of controller actions
    /// Logs before and after action execution for performance monitoring
    /// </summary>
    public class LogActionFilter : IActionFilter
    {
        private readonly ILogger<LogActionFilter> _logger;
        private Stopwatch _stopwatch;

        public LogActionFilter(ILogger<LogActionFilter> logger)
        {
            _logger = logger;
        }

        // Runs before the action method executes
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch = Stopwatch.StartNew();
            
            var actionName = context.ActionDescriptor.DisplayName;
            var controllerName = context.Controller.GetType().Name;
            var arguments = string.Join(", ", context.ActionArguments.Select(a => $"{a.Key}={a.Value}"));
            
            _logger.LogInformation(
                "[LogActionFilter] Executing {Controller}.{Action} with args: {Args}",
                controllerName,
                actionName,
                arguments);
        }

        // Runs after the action method executes
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch.Stop();
            
            var actionName = context.ActionDescriptor.DisplayName;
            var controllerName = context.Controller.GetType().Name;
            var statusCode = context.Result is Microsoft.AspNetCore.Mvc.ObjectResult objResult 
                ? objResult.StatusCode?.ToString() ?? "OK" 
                : "Unknown";
            
            _logger.LogInformation(
                "[LogActionFilter] Executed {Controller}.{Action} - Status: {StatusCode} - Duration: {DurationMs}ms",
                controllerName,
                actionName,
                statusCode,
                _stopwatch.ElapsedMilliseconds);
        }
    }
}
