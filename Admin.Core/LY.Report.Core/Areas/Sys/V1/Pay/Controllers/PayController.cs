using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Pay.Enum;
using LY.Report.Core.Service.Order.Info;
using LY.Report.Core.Service.Order.Info.Input;
using LY.Report.Core.Service.Order.Info.Output;
using LY.Report.Core.Service.Pay.Income;
using LY.Report.Core.Service.Pay.Income.Input;
using LY.Report.Core.Service.Pay.Income.Output;
using LY.Report.Core.Service.Pay.Refund;
using LY.Report.Core.Service.Pay.Refund.Input;
using LY.Report.Core.Service.Pay.Transfer;
using LY.Report.Core.Service.Pay.Transfer.Input;
using LY.Report.Core.Util.Common;
using Microsoft.AspNetCore.Http;

namespace LY.Report.Core.Areas.Sys.V1.Pay.Controllers
{
    /// <summary>
    /// 支付管理(统一支付)
    /// </summary>
    public class PayController : BaseAreaController
    {
        private readonly IUser _user;
        private readonly IPayIncomeService _payIncomeService;
        private readonly IPayRefundService _payRefundService;
        private readonly IPayTransferService _payTransferService;
        private readonly IOrderInfoService _orderInfoService;
        private readonly IHttpContextAccessor _context;

        public PayController(
            IUser user,
            IPayIncomeService payIncomeService,
            IPayRefundService payRefundService,
            IPayTransferService payTransferService,
            IOrderInfoService orderInfoService,
            IHttpContextAccessor context)
        {
            _user = user;
            _payIncomeService = payIncomeService;
            _payRefundService = payRefundService;
            _payTransferService = payTransferService;
            _orderInfoService = orderInfoService;
            _context = context;
        }

        /// <summary>
        /// 获取支付信息
        /// </summary>
        /// <param name="outTradeNo">商户单号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPayInfo(string outTradeNo)
        {
            if (outTradeNo.IsNull())
            {
                return ResponseOutput.NotOk("订单号不能为空");
            }

            var payIncomeRes = await _payIncomeService.GetOneAsync(new PayIncomeGetInput {OutTradeNo = outTradeNo});
            if (!payIncomeRes.Success)
            {
                return ResponseOutput.NotOk("支付订单不存在");
            }

            var payIncome = payIncomeRes.GetData<PayIncomeGetOutput>();
            if (payIncome == null || payIncome.OutTradeNo.IsNull())
            {
                return ResponseOutput.NotOk("支付订单不存在");
            }

            //是否微信浏览器
            var isWeChat = CommonHelper.IsWeChatPlatform(_context.HttpContext.Request.Headers["User-Agent"]);
            if (payIncome.PayOrderType == PayOrderType.Order)
            {
                var orderRes = await _orderInfoService.GetOneAsync(new OrderInfoGetInput { OutTradeNo = outTradeNo }); 
                if (!orderRes.Success)
                {
                    return ResponseOutput.NotOk("订单不存在");
                }

                var order = orderRes.GetData<OrderInfoFullGetOutput>();
                if (order == null || order.OutTradeNo.IsNull())
                {
                    return ResponseOutput.NotOk("订单不存在");
                }

                return ResponseOutput.Data(new {payOrder = payIncome, openId = isWeChat ? _user.WeChatOpenId : "", orderInfo = order});
            }

            return ResponseOutput.Data(new { payOrder = payIncome, openId = isWeChat ? _user.WeChatOpenId : ""});
        }

        /// <summary>
        /// 查询分页(支付)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPageIncome([FromQuery] PageInput<PayIncomeGetInput> model)
        {
            return await _payIncomeService.GetPageListAsync(model);
        }

        /// <summary>
        /// 查询分页(退款)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPageRefund([FromQuery] PageInput<PayRefundGetInput> model)
        {
            return await _payRefundService.GetPageListAsync(model);
        }

        /// <summary>
        /// 查询分页(转账)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPageTransfer([FromQuery] PageInput<PayTransferGetInput> model)
        {
            return await _payTransferService.GetPageListAsync(model);
        }
    }
}
