using LY.Report.Core.Attributes;
using LY.Report.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Sys.V1
{
    /// <summary>
    /// 基础API控制器
    /// </summary>
    [ApiController]
    [ValidatePermission]
    [ValidateInput]
    [VersionRoute(ApiVersion.S_V1)]
    [ApiExplorerSettings(GroupName ="S_V1")]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
