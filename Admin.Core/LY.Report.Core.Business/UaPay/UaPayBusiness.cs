using System;
using System.Threading.Tasks;
using AutoMapper;
using LY.Report.Core.Business.UaPay.Input;
using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.LYApiUtil.Pay;
using LY.Report.Core.LYApiUtil.Pay.In;
using LY.Report.Core.LYApiUtil.Pay.Out;
using LY.Report.Core.Model.Order.Enum;
using LY.Report.Core.Model.Pay.Enum;
using LY.Report.Core.Repository.Order;
using LY.Report.Core.Repository.Pay;
using FreeSql;
using ApiPayStatus = LY.Report.Core.LYApiUtil.Pay.Enum.PayStatus;
using ApiRefundStatus = LY.Report.Core.LYApiUtil.Pay.Enum.RefundStatus;
using ApiTransferStatus = LY.Report.Core.LYApiUtil.Pay.Enum.TransferStatus;

namespace LY.Report.Core.Business.UaPay
{
    public class UaPayBusiness : IUaPayBusiness
    {
        private readonly IMapper _mapper;
        private readonly IPayIncomeRepository _payIncomeRepository;
        private readonly IPayRefundRepository _payRefundRepository;
        private readonly IPayTransferRepository _payTransferRepository;
        private readonly IOrderInfoRepository _orderInfoRepository;
        private readonly IOrderDeliveryRepository _orderDeliveryRepository;
        private readonly AppConfig _appConfig;

        public UaPayBusiness(IMapper mapper,
            IPayIncomeRepository payIncomeRepository,
            IPayRefundRepository payRefundRepository,
            IPayTransferRepository payTransferRepository,
            IOrderInfoRepository orderInfoRepository,
            IOrderDeliveryRepository orderDeliveryRepository,
            AppConfig appConfig)
        {
            _mapper = mapper;
            _payIncomeRepository = payIncomeRepository;
            _payRefundRepository = payRefundRepository;
            _payTransferRepository = payTransferRepository;
            _orderInfoRepository = orderInfoRepository;
            _orderDeliveryRepository = orderDeliveryRepository;
            _appConfig = appConfig;
        }

        #region 用户资金
        public async Task<IResponseOutput> GetUserFundAsync(string userId)
        {
            var apiResult = await PayApiHelper.GetUserFundAsync(userId);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Ok(apiResult.Msg, apiResult.Data);
        }
        #endregion

        #region 下单
        public async Task<IResponseOutput> TradeAsync(UaPayTradeIn input)
        {
            input.AppFrontNotifyUrl = input.AppFrontNotifyUrl.IsNull() ? _appConfig.PayConfig.FrontNotifyUrl : input.AppFrontNotifyUrl;
            input.AppBackNotifyUrl = input.AppBackNotifyUrl.IsNull() ? _appConfig.PayConfig.BackNotifyUrl : input.AppBackNotifyUrl;
            input.AppQuitUrl = input.AppQuitUrl.IsNull() ? _appConfig.PayConfig.QuitUrl : input.AppQuitUrl;
            var apiResult = await PayApiHelper.TradeAsync(input);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Ok(apiResult.Msg,apiResult.Data);
        }

        public async Task<IResponseOutput> TradeQueryAsync(UaPayTradeQueryIn input)
        {
            var apiResult = await PayApiHelper.TradeQueryAsync(input);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg, apiResult.MsgCode);
            }
            return ResponseOutput.Ok(apiResult.Msg, apiResult.Data);
        }

        public async Task<IResponseOutput> TradeQueryPageAsync(PageInput<UaPayTradeQueryIn> input)
        {
            var apiResult = await PayApiHelper.TradeQueryPageAsync(input.Filter, input.CurrentPage, input.PageSize);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Ok(apiResult.Msg, apiResult.Data);
        }

        public async Task<IResponseOutput> SecuredTradeUnfreezeAsync(UaPayTradeUnfreezeIn input)
        {
            var apiResult = await PayApiHelper.SecuredTradeUnfreezeAsync(input);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Ok(apiResult.Msg);
        }
        #endregion

        #region 退款
        public async Task<IResponseOutput> TradeRefundAsync(UaPayTradeRefundIn input)
        {
            input.AppBackNotifyUrl = input.AppBackNotifyUrl.IsNull() ? _appConfig.PayConfig.BackNotifyUrl : input.AppBackNotifyUrl;
            var apiResult = await PayApiHelper.TradeRefundAsync(input);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Ok(apiResult.Msg);
        }

        public async Task<IResponseOutput> TradeRefundQueryAsync(UaPayTradeRefundQueryIn input)
        {
            var apiResult = await PayApiHelper.TradeRefundQueryAsync(input);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Ok(apiResult.Msg, apiResult.Data);
        }

        public async Task<IResponseOutput> TradeRefundQueryPageAsync(PageInput<UaPayTradeRefundQueryIn> input)
        {
            var apiResult = await PayApiHelper.TradeRefundQueryPageAsync(input.Filter, input.CurrentPage, input.PageSize);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Ok(apiResult.Msg, apiResult.Data);
        }
        #endregion

        #region 转账
        public async Task<IResponseOutput> TransferAsync(UaPayTransferIn input)
        {
            input.AppBackNotifyUrl = input.AppBackNotifyUrl.IsNull() ? _appConfig.PayConfig.BackNotifyUrl : input.AppBackNotifyUrl;
            var apiResult = await PayApiHelper.TransferAsync(input);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Ok(apiResult.Msg, apiResult.Data);
        }

        public async Task<IResponseOutput> TransferQueryAsync(UaPayTransferQueryIn input)
        {
            var apiResult = await PayApiHelper.TransferQueryAsync(input);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Ok(apiResult.Msg, apiResult.Data);
        }

        public async Task<IResponseOutput> TradeQueryPageAsync(PageInput<UaPayTransferQueryIn> input)
        {
            var apiResult = await PayApiHelper.TransferQueryPageAsync(input.Filter, input.CurrentPage, input.PageSize);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Ok(apiResult.Msg, apiResult.Data);
        }
        #endregion

        #region 异步通知

        /// <summary>
        /// 修改支付状态
        /// </summary>
        /// <param name="msgTradeResultIn"></param>
        /// <returns></returns>
        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> UpdateTradeStatusAsync(MsgTradeResultIn msgTradeResultIn)
        {
            if (string.IsNullOrEmpty(msgTradeResultIn.OutTradeNo))
            {
                return ResponseOutput.NotOk("参数错误");
            }
            var payIncome = await _payIncomeRepository.GetOneAsync(t=>t.OutTradeNo == msgTradeResultIn.OutTradeNo);
            if (string.IsNullOrEmpty(payIncome?.Id))
            {
                return ResponseOutput.NotOk("订单参数错误！");
            }

            if (msgTradeResultIn.PayStatus == ApiPayStatus.Unpaid || msgTradeResultIn.PayStatus == ApiPayStatus.Paying)
            {
                return ResponseOutput.Ok();
            }

            #region 修改支付状态
            _mapper.Map(msgTradeResultIn, payIncome);
            //修改担保交易状态
            if (payIncome.PayStatus == PayStatus.Paid)
            {
                payIncome.SecuredTradeStatus = payIncome.IsSecuredTrade ? SecuredTradeStatus.Trading : SecuredTradeStatus.Normal;
            }
            if (payIncome.PayStatus == PayStatus.Closed || payIncome.PayStatus == PayStatus.Failed)
            {
                payIncome.SecuredTradeStatus = payIncome.IsSecuredTrade ? SecuredTradeStatus.Closed : SecuredTradeStatus.Normal;
            }

            payIncome.IsCallBack = CallBack.CallSuccess;

            var res = await _payIncomeRepository.UpdateAsync(payIncome);
            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改支付状态失败");
            }
            #endregion

            //判断订单类型
            if (payIncome.PayOrderType == PayOrderType.Order)
            {
                return await UpdateOrderStatusAsync(msgTradeResultIn);
            }
            if (payIncome.PayOrderType == PayOrderType.Recharge)
            {
                return await UpdateRechargeStatusAsync(msgTradeResultIn);
            }
            if(payIncome.PayOrderType == PayOrderType.Deposit)
            {
                return await UpdateDepositStatusAsync(msgTradeResultIn);
            }

            return ResponseOutput.NotOk("订单类型错误");
        }

        #region 订单业务处理

        /// <summary>
        /// 处理订单状态
        /// </summary>
        /// <param name="msgTradeResultIn"></param>
        /// <returns></returns>
        private async Task<IResponseOutput> UpdateOrderStatusAsync(MsgTradeResultIn msgTradeResultIn)
        {
            var order = await _orderInfoRepository.GetOneAsync(t => t.OutTradeNo == msgTradeResultIn.OutTradeNo);
            if (order == null || order.OrderNo.IsNull())
            {
                return ResponseOutput.NotOk("订单不存在");
            }

            //是否更新 指派司机接单(外部订单) 状态
            var isUpdateOutsideOrder = msgTradeResultIn.PayStatus == ApiPayStatus.Paid && order.OutsideOrderNo.IsNotNull() && order.DriverId.IsNotNull();
            //是否取消订单
            var isCancelOrder = msgTradeResultIn.PayStatus == ApiPayStatus.Closed || msgTradeResultIn.PayStatus == ApiPayStatus.Failed;

            int res = await _orderInfoRepository.UpdateDiyAsync
                .SetIf(msgTradeResultIn.PayStatus == ApiPayStatus.Paid, t => t.OrderStatus, isUpdateOutsideOrder ? OrderStatus.Received : OrderStatus.Waiting)
                .SetIf(isUpdateOutsideOrder, t => t.ReceivedOrderDate, DateTime.Now)
                .SetIf(isUpdateOutsideOrder, t => t.PlanDeliveredOrderDate, Convert.ToDateTime(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")))//预计1天内送达;
                .SetIf(isCancelOrder, t => t.OrderStatus, OrderStatus.Cancelled)
                .SetIf(isCancelOrder, t => t.CancelStatus, CancelStatus.Cancelled)
                .SetIf(isCancelOrder, t => t.CancelReason, msgTradeResultIn.PayStatusMsg)
                .SetIf(isCancelOrder, t => t.CancelDate, DateTime.Now)
                .Where(t => t.OutTradeNo == msgTradeResultIn.OutTradeNo)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改订单状态失败");
            }

            res = await _orderDeliveryRepository.UpdateDiyAsync
                .SetIf(msgTradeResultIn.PayStatus == ApiPayStatus.Paid, t => t.OrderStatus, order.OutsideOrderNo.IsNull() ? OrderStatus.Waiting : OrderStatus.Received)
                .SetIf(isCancelOrder, t => t.OrderStatus, OrderStatus.Cancelled)
                .Where(t => t.OutTradeNo == msgTradeResultIn.OutTradeNo)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改订单配送状态失败");
            }

            return ResponseOutput.Ok("支付成功");
        }

        /// <summary>
        /// 处理充值状态
        /// </summary>
        /// <param name="msgTradeResultIn"></param>
        /// <returns></returns>
        private async Task<IResponseOutput> UpdateRechargeStatusAsync(MsgTradeResultIn msgTradeResultIn)
        {
            return ResponseOutput.Ok("充值成功");
        }

        /// <summary>
        /// 处理保证金状态
        /// </summary>
        /// <param name="msgTradeResultIn"></param>
        /// <returns></returns>
        private async Task<IResponseOutput> UpdateDepositStatusAsync(MsgTradeResultIn msgTradeResultIn)
        {
            return ResponseOutput.Ok("支付成功");
        }
        #endregion

        /// <summary>
        /// 修改退款状态
        /// </summary>
        /// <param name="msgRefundResultIn"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> UpdateRefundStatusAsync(MsgRefundResultIn msgRefundResultIn)
        {
            if (string.IsNullOrEmpty(msgRefundResultIn.OutTradeNo))
            {
                return ResponseOutput.NotOk("参数错误");
            }
            var payRefund = await _payRefundRepository.GetOneAsync(t=>t.RefundOutTradeNo == msgRefundResultIn.RefundOutTradeNo);
            if (string.IsNullOrEmpty(payRefund?.Id))
            {
                return ResponseOutput.NotOk("订单参数错误！");
            }

            if (msgRefundResultIn.RefundStatus == ApiRefundStatus.Unpaid || msgRefundResultIn.RefundStatus == ApiRefundStatus.Paying)
            {
                return ResponseOutput.Ok();
            }
            
            _mapper.Map(msgRefundResultIn, payRefund);
            payRefund.IsCallBack = CallBack.CallSuccess;
            var res = await _payRefundRepository.UpdateAsync(payRefund);
            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改支付状态失败");
            }

            return ResponseOutput.Ok("更新成功");
        }

        /// <summary>
        /// 修改转账状态
        /// </summary>
        /// <param name="msgTransferResultIn"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> UpdateTransferStatusAsync(MsgTransferResultIn msgTransferResultIn)
        {
            if (string.IsNullOrEmpty(msgTransferResultIn.TransferOutTradeNo))
            {
                return ResponseOutput.NotOk("参数错误");
            }
            var payTransfer = await _payTransferRepository.GetOneAsync(t=>t.TransferOutTradeNo == msgTransferResultIn.TransferOutTradeNo);
            if (string.IsNullOrEmpty(payTransfer?.Id))
            {
                return ResponseOutput.NotOk("订单参数错误！");
            }

            if (msgTransferResultIn.TransferStatus == ApiTransferStatus.Unpaid || msgTransferResultIn.TransferStatus == ApiTransferStatus.Paying)
            {
                return ResponseOutput.Ok();
            }
            
            _mapper.Map(msgTransferResultIn, payTransfer);
            //修改担保交易状态
            payTransfer.IsCallBack = CallBack.CallSuccess;

            var res = await _payTransferRepository.UpdateAsync(payTransfer);
            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改转账状态失败");
            }

            return ResponseOutput.Ok("更新成功");
        }

        #endregion

        #region 检查支付状态

        /// <summary>
        /// 检查订单实际支付状态
        /// </summary>
        /// <returns></returns>
        public async Task<IResponseOutput> CheckPayStatusAsync(string outTradeNo)
        {
            var payStatusRes = await TradeQueryAsync(new UaPayTradeQueryIn { OutTradeNo = outTradeNo });
            if (!payStatusRes.Success)
            {
                return payStatusRes;
            }
            var tradeQueryGetOutput = payStatusRes.GetData<TradeQueryGetOutput>();
            if(tradeQueryGetOutput.PayStatus == ApiPayStatus.Paid)
            {
                var msgTradeResultIn = _mapper.Map<MsgTradeResultIn>(tradeQueryGetOutput);
                var res = await UpdateTradeStatusAsync(msgTradeResultIn);
                if (!res.Success)
                {
                    return ResponseOutput.NotOk("更新支付状态错误");
                }
            }
            var payIncome = await _payIncomeRepository.GetOneAsync(t => t.OutTradeNo == outTradeNo);

            return ResponseOutput.Ok("查询成功", payIncome);
        }
        #endregion
    }
}
