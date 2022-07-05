using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Sys.V1.Order
{
    /// <summary>
    /// 域控制器
    /// </summary>
    [Area("Order")]
    //[NoPermission]
    public abstract class BaseAreaController : BaseApiController
    {
    }
}