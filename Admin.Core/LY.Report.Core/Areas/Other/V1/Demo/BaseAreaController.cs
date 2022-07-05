using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Other.V1.Demo.Demo
{
    /// <summary>
    /// 域控制器
    /// </summary>
    [Area("Demo")]
    //[NoPermission]
    public abstract class BaseAreaController : BaseApiController
    {
    }
}