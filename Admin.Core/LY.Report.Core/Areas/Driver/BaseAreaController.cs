using EonUp.Delivery.Core.Attributes;
using EonUp.Delivery.Core.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Driver
{
    /// <summary>
    /// 域控制器
    /// </summary>
    [Area("Driver")]
    //[NoPermission]
    public abstract class BaseAreaController : BaseApiController
    {
    }
}