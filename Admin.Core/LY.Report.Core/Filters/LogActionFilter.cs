using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using LY.Report.Core.Attributes;
using LY.Report.Core.Helper;

namespace LY.Report.Core.Filters
{
    public class LogActionFilter : IAsyncActionFilter
    {
        private readonly ILogHandler _logHandler;

        public LogActionFilter(ILogHandler logHandler)
        {
            _logHandler = logHandler;
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionDescriptor.EndpointMetadata.Any(m => m.GetType() == typeof(NoOperationLogAttribute)))
            {
                return next();
            }

            if (context.ActionDescriptor.AttributeRouteInfo == null)
            {
                return next();
            }
            return _logHandler.LogAsync(context, next);
        }
    }
}
