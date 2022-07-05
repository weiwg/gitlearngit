using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.Order.FreightCalc;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Order.Controllers
{
    /// <summary>
    /// 计价管理
    /// </summary>
    public class OrderFreightCalcController : BaseAreaController
    {
        private readonly IOrderFreightCalcService _orderFreightCalcService;

        public OrderFreightCalcController(IOrderFreightCalcService orderFreightCalcService)
        {
            _orderFreightCalcService = orderFreightCalcService;
        }

        #region 查询
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string orderNo)
        {
            return await _orderFreightCalcService.GetOneAsync(orderNo);
        }
        #endregion
    }
}
