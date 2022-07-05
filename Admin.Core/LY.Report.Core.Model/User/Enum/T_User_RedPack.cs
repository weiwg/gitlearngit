using System.ComponentModel;

namespace LY.Report.Core.Model.User.Enum
{
    /// <summary>
    /// 红包状态
    /// </summary>
    public enum UserRedPackStatus
    {
        /// <summary>
        /// 未使用(可使用)
        /// </summary>
        [Description("未使用")]
        Unused = 1,
        /// <summary>
        /// 已使用
        /// </summary>
        [Description("已使用")]
        Used = 2,
        /// <summary>
        /// 未生效
        /// </summary>
        [Description("未生效")]
        UnActivated = 3,
        /// <summary>
        /// 已过期
        /// </summary>
        [Description("已过期")]
        Expired = 4
    }

}
