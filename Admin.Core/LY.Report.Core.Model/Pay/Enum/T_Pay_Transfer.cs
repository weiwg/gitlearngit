using System;
using System.ComponentModel;

namespace LY.Report.Core.Model.Pay.Enum
{
    /// <summary>
    /// 转账类型
    /// </summary>
    [Serializable]
    public enum TransferType
    {
        /// <summary>
        /// 提现
        /// </summary>
        [Description("提现")]
        Withdraw = 1,
        /// <summary>
        /// 转账
        /// </summary>
        [Description("转账")]
        Transfer = 2,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 99
    }

    /// <summary>
    /// 转账状态
    /// </summary>
    [Serializable]
    public enum TransferStatus
    {
        /// <summary>
        /// 待转账
        /// </summary>
        [Description("待转账")]
        Unpaid = 1,
        /// <summary>
        /// 已转账(成功)
        /// </summary>
        [Description("已转账")]
        Paid = 2,
        /// <summary>
        /// 已关闭
        /// </summary>
        [Description("已关闭")]
        Closed = 3,
        /// <summary>
        /// 转账失败
        /// </summary>
        [Description("转账失败")]
        Failed = 4,
        /// <summary>
        /// 转账中(除明确状态外的其他状态)
        /// </summary>
        [Description("转账中")]
        Paying = 9
    }
}
