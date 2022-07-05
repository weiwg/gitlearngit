using Microsoft.AspNetCore.Mvc;
using LY.Report.Core.Attributes;

namespace LY.Report.Core.Controllers.Base
{
    /// <summary>
    /// 基础API控制器
    /// </summary>
    [Route("Api/[area]/[controller]/[action]")]
    [ApiController]
    [ValidatePermission]
    [ValidateInput]
    //[VersionRoute(ApiVersion.V1)]
    public abstract class BaseApiController : ControllerBase
    {
    }
}