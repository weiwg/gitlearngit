using System.Threading.Tasks;
using LY.Report.Core.Attributes;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Service.Delivery.CarType;
using LY.Report.Core.Service.Delivery.CarType.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Mobile.V1.Delivery.Controllers
{
    /// <summary>
    /// 车型配置
    /// </summary>
    public class DeliveryCarTypeController : BaseAreaController
    {
        private readonly IDeliveryCarTypeService _deliveryCarTypeService;

        public DeliveryCarTypeController(IDeliveryCarTypeService deliveryCarTypeService)
        {
            _deliveryCarTypeService = deliveryCarTypeService;
        }

        /// <summary>
        /// 查询所有可用车型
        /// </summary>
        /// <returns></returns>
        [AllowEupApi]
        [HttpGet]
        public async Task<IResponseOutput> GetCarType()
        {
            return await _deliveryCarTypeService.GetListAsync(new DeliveryCarTypeGetInput{ IsActive = IsActive.Yes });
        }
    }
}
