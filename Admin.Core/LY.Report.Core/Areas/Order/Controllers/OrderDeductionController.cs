using EonUp.Delivery.Core.Service.Order.Deduction;

namespace EonUp.Delivery.Core.Areas.Order.Controllers
{
    /// <summary>
    /// 抵扣管理
    /// </summary>
    public class OrderDeductionController : BaseAreaController
    {
        private readonly IOrderDeductionService _orderDeductionService;

        public OrderDeductionController(IOrderDeductionService orderDeductionService)
        {
            _orderDeductionService = orderDeductionService;
        }
    }
}
