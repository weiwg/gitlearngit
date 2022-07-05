using System.ComponentModel;

namespace LY.Report.Core.Model.User.Enum
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1,
        /// <summary>
        /// 锁定
        /// </summary>
        [Description("锁定")]
        Lock = 2
    }
}
