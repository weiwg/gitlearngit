using System;
using LY.Report.Core.Model.Pay.Enum;

namespace LY.Report.Core.Service.Pay.Income.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class PayIncomeGetInput
    {
        /// <summary>
        /// 商户单号
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 平台单号
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 支付订单类型
        /// </summary>
        public PayOrderType PayOrderType { get; set; }
        
        /// <summary>
        /// 支付状态
        /// </summary>
        public PayStatus PayStatus { get; set; }

        /// <summary>
        /// 是否回写
        /// </summary>
        public CallBack IsCallBack { get; set; }

        /// <summary>
        /// 交易开始时间(格式2021-01-01)
        /// 交易开始时间+开始结束时间需同时填写,否则无效
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 交易结束时间(格式2021-01-01)
        /// 交易开始时间+开始结束时间需同时填写,否则无效
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
