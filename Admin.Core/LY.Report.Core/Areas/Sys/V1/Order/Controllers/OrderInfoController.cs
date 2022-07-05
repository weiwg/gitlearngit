using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Order.Info;
using LY.Report.Core.Service.Order.Info.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Sys.V1.Order.Controllers
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public class OrderInfoController : BaseAreaController
    {
        private readonly IOrderInfoService _orderInfoService;

        public OrderInfoController(IOrderInfoService orderInfoService)
        {
            _orderInfoService = orderInfoService;
        }

        #region 查询
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get([FromQuery] OrderInfoGetInput input)
        {
            return await _orderInfoService.GetOneAsync(input);
        }
        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<OrderInfoGetInput> model)
        {
            return await _orderInfoService.GetPageListAsync(model);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 后台取消订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdateSysCancelOrder(OrderInfoCancelOrderInput input)
        {
            return await _orderInfoService.UpdateSysCancelOrderAsync(input);
        }
        #endregion
    }
}
