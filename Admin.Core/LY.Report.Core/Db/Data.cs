using LY.Report.Core.Model.Auth;
using LY.Report.Core.Model.System;
using LY.Report.Core.Model.User;

namespace LY.Report.Core.Db
{
    /// <summary>
    /// 数据
    /// </summary>
    public class Data
    {
        public AuthApi[] Apis { get; set; }
        public AuthView[] Views { get; set; }
        public AuthPermission[] Permissions { get; set; }
        public UserInfo[] Users { get; set; }
        public AuthRole[] Roles { get; set; }
        public AuthUserRole[] UserRoles { get; set; }
        public AuthRolePermission[] RolePermissions { get; set; }
        public SysTenant[] Tenants { get; set; }
    }
}
