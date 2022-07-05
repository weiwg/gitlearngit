using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace LY.Report.Core.Util.Middleware
{
    /// <summary>
    /// 手动获取注入对象
    /// </summary>
    public static class HttpService
    {
        /// <summary>
        /// 注入对象服务提供类
        /// </summary>
        public static IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 手动获取注入的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>() where T : class
        {
            var httpContext = ServiceProvider?.GetService<IHttpContextAccessor>()?.HttpContext;
            try
            {
                if (httpContext != null)
                {
                    return httpContext.RequestServices.GetRequiredService<T>();
                }
                return ServiceProvider?.GetRequiredService<T>();
            }
            catch (Exception)
            {
                //log
                return null;
            }
        }
    }
}
