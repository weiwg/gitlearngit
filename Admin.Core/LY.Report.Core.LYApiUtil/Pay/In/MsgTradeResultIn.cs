using System;
using LY.Report.Core.LYApiUtil.Pay.Enum;

namespace LY.Report.Core.LYApiUtil.Pay.In
{
    /// <summary>
    /// 支付状态
    /// </summary>
    public class MsgTradeResultIn
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
        /// 支付平台
        /// </summary>
        public FundPlatform FundPlatform { get; set; }

        /// <summary>
        /// 支付平台描述
        /// </summary>
        public string FundPlatformDescribe { get; set; }

        /// <summary>
        /// 实际App交易手续费(包括平台手续费)
        /// </summary>
        public decimal ActualSettlePayAppCharge { get; set; }

        /// <summary>
        /// 实际结算平台交易手续费, 用于计算支付平台扣除的手续费
        /// </summary>
        public decimal ActualSettlePayPlatformCharge { get; set; }

        /// <summary>
        /// 实际支付金额(支付平台返回)
        /// </summary>
        public decimal ActualPayAmount { get; set; }

        /// <summary>
        /// 实际App补贴金额
        /// </summary>
        public decimal ActualSettleAppSubsidyAmount { get; set; }

        /// <summary>
        /// 实际结算金额(ActualPayAmount-ActualSettlePayPlatformCharge)
        /// </summary>
        public decimal ActualSettleAmount { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayDate { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public PayStatus PayStatus { get; set; }

        /// <summary>
        /// 支付状态描述
        /// </summary>
        public string PayStatusDescribe { get; set; }

        /// <summary>
        /// 支付状态码
        /// </summary>
        public string PayStatusCode { get; set; }

        /// <summary>
        /// 支付状态消息
        /// </summary>
        public string PayStatusMsg { get; set; }
    }
}
