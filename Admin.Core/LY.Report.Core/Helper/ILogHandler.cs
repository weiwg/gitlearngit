using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LY.Report.Core.Helper
{
    /// <summary>
    /// 操作日志处理接口
    /// </summary>
    public interface ILogHandler
    {
        /// <summary>
        /// 写操作日志
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        Task LogAsync(ActionExecutingContext context, ActionExecutionDelegate next);
    }
}
