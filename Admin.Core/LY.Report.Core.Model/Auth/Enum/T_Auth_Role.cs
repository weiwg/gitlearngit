using System.ComponentModel;

namespace LY.Report.Core.Model.Auth.Enum
{
    /// <summary>
    /// 角色枚举
    /// </summary>
    public enum RoleType
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        [Description("SuperAdmin")]
        SuperAdmin = 999,
        /// <summary>
        /// 系统管理员
        /// </summary>
        [Description("Admin")]
        Admin = 100,
        /// <summary>
        /// 平台用户
        /// </summary>
        [Description("User")]
        User = 200

    }
}
