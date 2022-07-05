using System;
using System.Threading.Tasks;
using LY.Report.Core.Business.Order;
using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Common.Output;
using LY.Report.Core.LYApiUtil.Pay;
using LY.Report.Core.LYApiUtil.Pay.In;
using LY.Report.Core.Model.Order.Enum;
using LY.Report.Core.Model.Pay.Enum;
using LY.Report.Core.Repository.Order;
using LY.Report.Core.Repository.Pay;
using LY.Report.Core.Util.Common;
using FreeSql;

namespace LY.Report.Core.Business.Pay
{
    public class PayBusiness : IPayBusiness
    {
        private readonly IPayIncomeRepository _payIncomeRepository;
        private readonly IOrderInfoRepository _orderInfoRepository;
        private readonly IOrderBusiness _orderBusiness;
        private readonly LogHelper _logger = new LogHelper("PayBusiness");

        public PayBusiness(
            IPayIncomeRepository payIncomeRepository,
            IOrderInfoRepository orderInfoRepository,
            IOrderBusiness orderBusiness)
        {
            _payIncomeRepository = payIncomeRepository;
            _orderInfoRepository = orderInfoRepository;
            _orderBusiness = orderBusiness;
        }

        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> CloseOutTradeAsync(string outTradeNo, PayOrderType payOrderType)
        {
            //操作关闭交易
            var tradeCloseIn = new TradeCloseIn { OutTradeNo = outTradeNo };
            var payApiResult = await PayApiHelper.TradeCloseAsync(tradeCloseIn);
            if (!payApiResult.Success)
            {
                _logger.Error($"定时取消支付订单,查询订单状态错误:Msg{payApiResult.Msg},OutTradeNo:{outTradeNo}");
                return ResponseOutput.NotOk(payApiResult.Msg);
            }

            return await CloseTradeAsync(outTradeNo, payOrderType);
        }

        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> CloseTradeAsync(string outTradeNo, PayOrderType payOrderType)
        {
            //修改支付状态
            int res = await _payIncomeRepository.UpdateDiyAsync
                .Set(t => t.PayStatus, PayStatus.Closed)
                .Set(t => t.PayStatusCode, "Overtime_Unpaid")
                .Set(t => t.PayStatusMsg, "超时未支付")
                .Where(t => t.OutTradeNo == outTradeNo)
                .ExecuteAffrowsAsync();
            if (res <= 0)
            {
                //跳过此次操作
                _logger.Error("定时取消支付订单,修改支付状态错误,OutTradeNo:" + outTradeNo);
                return ResponseOutput.NotOk("修改支付状态");
            }

            //关闭订单
            #region 关闭订单
            if (payOrderType == PayOrderType.Order)
            {
                var order = await _orderInfoRepository.GetOneAsync(t => t.OutTradeNo == outTradeNo);
                if (order == null || order.OrderNo.IsNull())
                {
                    //跳过此次操作
                    _logger.Error("定时取消订单,订单不存在,OutTradeNo:" + outTradeNo);
                    return ResponseOutput.NotOk("订单不存在");
                }
                res = await _orderInfoRepository.UpdateDiyAsync
                    .Set(t => t.OrderStatus, OrderStatus.Cancelled)
                    .Set(t => t.CancelStatus, CancelStatus.Cancelled)
                    .Set(t => t.CancelReason, "超时未支付")
                    .Set(t => t.CancelDate, DateTime.Now)
                    .Where(t => t.OrderNo == order.OrderNo)
                    .ExecuteAffrowsAsync();
                if (res <= 0)
                {
                    return ResponseOutput.NotOk("取消订单失败");
                }

                //退回抵扣
                var resOrderDeduction = await _orderBusiness.OrderRefundDeductionAsync(order.OrderNo);
                if (!resOrderDeduction.Success)
                {
                    return resOrderDeduction;
                }
            }
            #endregion
            return ResponseOutput.Ok("取消成功");
        }
    }
}
