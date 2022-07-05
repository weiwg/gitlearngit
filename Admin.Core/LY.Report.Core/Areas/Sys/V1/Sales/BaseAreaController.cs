using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Sys.V1.Sales
{
    /// <summary>
    /// 域控制器
    /// </summary>
    [Area("Sales")]
    //[NoPermission]
    public abstract class BaseAreaController : BaseApiController
    {
    }
}