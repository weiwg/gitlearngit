using System;
using LY.Report.Core.LYApiUtil.Pay.Enum;

namespace LY.Report.Core.LYApiUtil.Pay.Out
{
    /// <summary>
    /// 支付状态
    /// </summary>
    public class TradeResult 
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
        /// 交易手续费, 用于计算支付平台扣除的手续费
        /// </summary>
        public decimal PayCharge { get; set; }

        /// <summary>
        /// 实际支付金额
        /// </summary>
        public decimal ActualPayAmount { get; set; }

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
