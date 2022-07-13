using System.ComponentModel;

namespace LY.Report.Core.Model.Product.Enum
{
    /// <summary>
    /// 异常状态
    /// </summary>
    public enum AbnormalStatus
    {
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 0,
        /// <summary>
        /// 已处理
        /// </summary>
        [Description("已处理")]
        Processed = 1,
        /// <summary>
        /// 未处理
        /// </summary>
        [Description("未处理")]
        Unhandled = 2,
    }
}
