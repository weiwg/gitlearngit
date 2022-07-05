using LY.Report.Core.Attributes;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Order.Enum;
using LY.Report.Core.Service.Order.Info;
using LY.Report.Core.Service.Order.Info.Input;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LY.Report.Core.Areas.Open.V1.Order.Controllers
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public class OrderInfoController : BaseAreaController
    {
        private readonly IOrderInfoService _orderInfoService;
        private readonly IUser _user;

        public OrderInfoController(IOrderInfoService orderInfoService, IUser user)
        {
            _orderInfoService = orderInfoService;
            _user = user;
        }

        #region 新增
        /// <summary>
        /// 新增(外部发单)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> AddOutside(OrderInfoAddFullInput input)
        {
            input.OrderType = OrderType.Store;
            return await _orderInfoService.AddFullAsync(input);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 用户外部订单取消
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> UserCancelOutsideOrder(OrderInfoCancelOutsideOrderInput input)
        {
            return await _orderInfoService.UserCancelOutsideOrderAsync(input);
        }
        #endregion
    }
}
