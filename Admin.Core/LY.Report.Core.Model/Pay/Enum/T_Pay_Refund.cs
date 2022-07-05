using System.ComponentModel;

namespace LY.Report.Core.Model.Pay.Enum
{
    /// <summary>
    /// 退款状态
    /// </summary>
    public enum RefundStatus
    {
        /// <summary>
        /// 待退款
        /// </summary>
        [Description("待退款")]
        Unpaid = 1,
        /// <summary>
        /// 已退款
        /// </summary>
        [Description("已退款")]
        Paid = 2,
        /// <summary>
        /// 已关闭
        /// </summary>
        [Description("已关闭")]
        Closed = 3,
        /// <summary>
        /// 退款失败
        /// </summary>
        [Description("退款失败")]
        Failed = 4,
        /// <summary>
        /// 支付中(除明确状态外的其他状态)
        /// </summary>
        [Description("退款中")]
        Paying = 9
    }
}
