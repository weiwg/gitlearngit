using LY.Report.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Other.V1.Mq
{
    /// <summary>
    /// 域控制器
    /// </summary>
    [Area("Mq")]
    [NoPermission]
    public abstract class BaseAreaController : BaseApiController
    {
    }
}