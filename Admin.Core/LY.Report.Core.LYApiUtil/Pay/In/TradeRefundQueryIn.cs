using System;

namespace LY.Report.Core.LYApiUtil.Pay.In
{
    /// <summary>
    /// 查询
    /// </summary>
    public class TradeRefundQueryIn
    {
        /// <summary>
        /// 用户Id
        /// 选填
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 商户单号(原单号)
        /// 选填
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 商户退款单号
        /// 选填
        /// </summary>
        public string RefundOutTradeNo { get; set; }

        /// <summary>
        /// 退款平台单号
        /// 选填
        /// </summary>
        public string RefundTradeNo { get; set; }

        /// <summary>
        /// 退款开始时间(格式2021-01-01)
        /// 下单结束时间+下单结束时间需同时填写,否则无效
        /// 选填
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 退款结束时间
        /// 下单结束时间+下单结束时间需同时填写,否则无效
        /// 选填
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
