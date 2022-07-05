using EonUp.Delivery.Core.Attributes;
using EonUp.Delivery.Core.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Pay
{
    /// <summary>
    /// 域控制器
    /// </summary>
    [Area("Pay")]
    //[NoPermission]
    public abstract class BaseAreaController : BaseApiController
    {
    }
}