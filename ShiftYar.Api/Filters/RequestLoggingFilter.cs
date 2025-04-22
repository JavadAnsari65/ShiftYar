using Microsoft.AspNetCore.Mvc.Filters;

namespace ShiftYar.Api.Filters
{
    ///RequestLoggingFilter: برای ثبت لاگ هنگام اجرای اکشن‌ها
    public class RequestLoggingFilter : IActionFilter
    {
        private readonly ILogger<RequestLoggingFilter> _logger;

        public RequestLoggingFilter(ILogger<RequestLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.ActionDescriptor.DisplayName;
            _logger.LogInformation($"➡ شروع اکشن: {action}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var action = context.ActionDescriptor.DisplayName;
            _logger.LogInformation($"⬅ پایان اکشن: {action}");
        }
    }
}
