using LY.Report.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Controllers.Base
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    [NoOperationLog]
    public abstract class BaseController : ControllerBase
    {
    }
}