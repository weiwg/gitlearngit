using EonUp.Delivery.Core.Attributes;
using EonUp.Delivery.Core.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.User
{
    /// <summary>
    /// 域控制器
    /// </summary>
    [Area("User")]
    //[NoPermission]
    public abstract class BaseAreaController : BaseApiController
    {
    }
}