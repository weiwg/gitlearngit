using LY.Report.Core.Attributes;
using LY.Report.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Mobile.V1
{
    /// <summary>
    /// 基础API控制器
    /// </summary>
    [ApiController]
    [ValidatePermission]
    [ValidateInput]
    [VersionRoute(ApiVersion.M_V1)]
    [ApiExplorerSettings(GroupName = "M_V1")]
    public abstract class BaseApiController : ControllerBase
    {
    }
}
