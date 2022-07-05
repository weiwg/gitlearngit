using System;
using LY.Report.Core.LYApiUtil.Pay.Enum;

namespace LY.Report.Core.LYApiUtil.Pay.Out
{
    public class TradeRefundQueryGetOutput
    {
        /// <summary>
        /// 商户退款单号
        /// </summary>
        public string RefundOutTradeNo { get; set; }

        /// <summary>
        /// 商户单号(原单号)
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 平台退款单号
        /// </summary>
        public string RefundTradeNo { get; set; }

        /// <summary>
        /// 平台单号(原单号)
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 退款平台
        /// </summary>
        public FundPlatform FundPlatform { get; set; }

        /// <summary>
        /// 退款平台描述
        /// </summary>
        public string FundPlatformDescribe { get; set; }

        /// <summary>
        /// 退款说明
        /// </summary>
        public string RefundDescription { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RefundAmount { get; set; }

        /// <summary>
        /// 退款手续费
        /// </summary>
        public decimal RefundCharge { get; set; }

        /// <summary>
        /// 实际退款金额
        /// </summary>
        public decimal ActualRefundAmount { get; set; }

        /// <summary>
        /// 退款时间
        /// </summary>
        public DateTime? RefundDate { get; set; }

        /// <summary>
        /// 退款状态
        /// </summary>
        public RefundStatus RefundStatus { get; set; }

        /// <summary>
        /// 退款状态描述
        /// </summary>
        public string RefundStatusDescribe { get; set; }

        /// <summary>
        /// 退款状态码
        /// </summary>
        public string RefundStatusCode { get; set; }

        /// <summary>
        /// 退款状态消息
        /// </summary>
        public string RefundStatusMsg { get; set; }
    }
}
