using System;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EonUp.Delivery.Core.Attributes;
using EonUp.Delivery.Core.Business.UaPay;
using EonUp.Delivery.Core.Common.Auth;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.EupApiUtil.Pay;
using EonUp.Delivery.Core.EupApiUtil.Pay.In;
using EonUp.Delivery.Core.Model.Pay.Enum;
using EonUp.Delivery.Core.Service.Order.Info;
using EonUp.Delivery.Core.Service.Order.Info.Input;
using EonUp.Delivery.Core.Service.Order.Info.Output;
using EonUp.Delivery.Core.Service.Pay.Income;
using EonUp.Delivery.Core.Service.Pay.Income.Input;
using EonUp.Delivery.Core.Service.Pay.Income.Output;
using EonUp.Delivery.Core.Service.Pay.Refund;
using EonUp.Delivery.Core.Service.Pay.Refund.Input;
using EonUp.Delivery.Core.Service.Pay.Transfer;
using EonUp.Delivery.Core.Service.Pay.Transfer.Input;
using EonUp.Delivery.Core.Util.Common;
using EonUp.Delivery.Core.Util.Tool;
using EonUp.Delivery.Core.Service.Pay.UaTrade.Input;
using EonUp.Delivery.Core.Service.Pay.UaTrade;
using Microsoft.AspNetCore.Http;

namespace EonUp.Delivery.Core.Areas.Pay.Controllers
{
    /// <summary>
    /// 支付管理(统一支付)
    /// </summary>
    public class PayController : BaseAreaController
    {
        private readonly IUser _user;
        private readonly IUaPayBusiness _uaPayBusiness;
        private readonly IPayIncomeService _payIncomeService;
        private readonly IPayRefundService _payRefundService;
        private readonly IPayTransferService _payTransferService;
        private readonly IOrderInfoService _orderInfoService;
        private readonly IPayUaTradeService _payUaTradeService;
        private readonly IHttpContextAccessor _context;
        private readonly ILogHelper _logger;

        public PayController(IUaPayBusiness uaPayBusiness, 
            IUser user,
            IPayIncomeService payIncomeService,
            IPayRefundService payRefundService,
            IPayUaTradeService payUaTradeService,
            IPayTransferService payTransferService,
            IOrderInfoService orderInfoService,
            IHttpContextAccessor context)
        {
            _logger = new LogHelper("PayController");
            _user = user;
            _uaPayBusiness = uaPayBusiness;
            _payIncomeService = payIncomeService;
            _payRefundService = payRefundService;
            _payTransferService = payTransferService;
            _orderInfoService = orderInfoService;
            _payUaTradeService = payUaTradeService;
            _context = context;
        }

        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> PayOrder(PayOrderAddInput input)
        {
            return await _payUaTradeService.PayOrderAsync(input);
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
        /// 支付结果接收
        /// </summary>
        /// <returns></returns>
        [AllowEupApi]
        [HttpPost]
        public IActionResult PayResult(PayApiResult<Hashtable> result)
        {
            var resultStr = NtsJsonHelper.GetJsonStr(result);
            _logger.Debug("PayResult:" + resultStr);
            if (!result.Success)
            {
                return Content("ok");
            }

            if (result.Code == 1)
            {
                try
                {
                    IResponseOutput res = null;
                    //支付结果
                    if (result.MsgCode == "PayResult")
                    {
                        var data = DataConvert.HashtableToObject<MsgTradeResultIn>(result.Data);
                        res = _uaPayBusiness.UpdateTradeStatusAsync(data).Result;
                    }
                    //退款结果
                    else if (result.MsgCode == "RefundResult")
                    {
                        var data = DataConvert.HashtableToObject<MsgRefundResultIn>(result.Data);
                        res = _uaPayBusiness.UpdateRefundStatusAsync(data).Result;
                    }
                    //转账结果
                    else if (result.MsgCode == "TransferResult")
                    {
                        var data = DataConvert.HashtableToObject<MsgTransferResultIn>(result.Data);
                        res = _uaPayBusiness.UpdateTransferStatusAsync(data).Result;
                    }

                    if (res != null && !res.Success)
                    {
                        _logger.Error("处理错误:" + res.Msg + ",\r\nPayResult:" + resultStr);
                    }
                }
                catch (Exception e)
                {
                    _logger.Error("处理错误:" + e.Message + ",\r\nPayResult:" + resultStr);
                    return Content("fail");
                }
            }

            return Content("ok");
        }

        /// <summary>
        /// 查询分页(支付)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPageIncome(PageInput<PayIncomeGetInput> model)
        {
            return await _payIncomeService.GetPageListAsync(model);
        }

        /// <summary>
        /// 查询分页(退款)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPageRefund(PageInput<PayRefundGetInput> model)
        {
            return await _payRefundService.GetPageListAsync(model);
        }

        /// <summary>
        /// 查询分页(转账)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPageTransfer(PageInput<PayTransferGetInput> model)
        {
            return await _payTransferService.GetPageListAsync(model);
        }
    }
}
