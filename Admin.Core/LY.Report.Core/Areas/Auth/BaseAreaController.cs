using EonUp.Delivery.Core.Attributes;
using EonUp.Delivery.Core.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Auth
{
    /// <summary>
    /// 域控制器
    /// </summary>
    [Area("Auth")]
    //[NoPermission]
    public class BaseAreaController: BaseApiController
    {
    }
}
