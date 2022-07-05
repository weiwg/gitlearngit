using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.Order.Delivery;
using EonUp.Delivery.Core.Service.Order.Delivery.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Order.Controllers
{
    /// <summary>
    /// 订单配送管理
    /// </summary>
    public class OrderDeliveryController : BaseAreaController
    {
        private readonly IOrderDeliveryService _orderDeliveryService;

        public OrderDeliveryController(IOrderDeliveryService orderDeliveryService)
        {
            _orderDeliveryService = orderDeliveryService;
        }

        #region 查询
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetList(OrderDeliveryGetInput input)
        {
            return await _orderDeliveryService.GetListAsync(input);
        }
        #endregion
    }
}
