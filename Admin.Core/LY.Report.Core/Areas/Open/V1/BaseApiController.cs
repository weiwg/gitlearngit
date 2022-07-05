using LY.Report.Core.Attributes;
using LY.Report.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Open.V1
{
    /// <summary>
    /// 基础API控制器
    /// </summary>
    [AllowEupApi]
    [ApiController]
    [ValidatePermission]
    [ValidateInput]
    [VersionRoute(ApiVersion.Open_V1)]
    [ApiExplorerSettings(GroupName = "Open_V1")]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
