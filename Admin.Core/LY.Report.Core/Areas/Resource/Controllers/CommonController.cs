using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EonUp.Delivery.Core.Common.Output;

namespace EonUp.Delivery.Core.Areas.Resource.Controllers
{
    /// <summary>
    /// 资源管理
    /// </summary>
    public class CommonController : BaseAreaController
    {
        private IHttpContextAccessor _accessor;

        public CommonController(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        /// <summary>
        /// 获取Ip
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseOutput GetIp()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            var ipV6 = HttpContext.Connection.RemoteIpAddress?.MapToIPv6().ToString();
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ip = Request.Headers["X-Forwarded-For"];
            }

            return ResponseOutput.Data(new {ip = ip, ipV6 = ipV6});
        }
    }
}
