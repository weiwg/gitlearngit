using System;
using LY.Report.Core.Model.Pay.Enum;

namespace LY.Report.Core.Service.Pay.Refund.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class PayRefundGetInput
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
        /// 退款状态
        /// </summary>
        public RefundStatus RefundStatus { get; set; }

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
