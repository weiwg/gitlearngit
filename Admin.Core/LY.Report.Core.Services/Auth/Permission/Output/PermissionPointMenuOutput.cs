using System.Collections.Generic;
using LY.Report.Core.Service.Auth.Auth.Output;

namespace LY.Report.Core.Service.Auth.Permission.Output
{
    /// <summary>
    /// 权限点和菜单信息
    /// </summary>
    public class PermissionPointMenuOutput
    {
        /// <summary>
        /// 用户菜单
        /// </summary>
        public List<AuthUserMenuDto> Menus { get; set; }

        /// <summary>
        /// 用户权限点
        /// </summary>
        public List<string> Permissions { get; set; }
    }
}
