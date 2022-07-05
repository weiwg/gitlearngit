using System;
using System.ComponentModel;

namespace LY.Report.Core.Model.Fund.Enum
{
    /// <summary>
    /// 充值状态
    /// </summary>
    [Serializable]
    public enum RechargeStatus
    {
        /// <summary>
        /// 待支付
        /// </summary>
        [Description("待支付")]
        Unpaid = 1,
        /// <summary>
        /// 已支付
        /// </summary>
        [Description("已支付")]
        Paid = 2,
        /// <summary>
        /// 已关闭
        /// </summary>
        [Description("已关闭")]
        Closed = 3
    }
}
