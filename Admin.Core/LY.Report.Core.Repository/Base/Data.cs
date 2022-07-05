using LY.Report.Core.Model.Auth;
using LY.Report.Core.Model.Personnel;
using LY.Report.Core.Model.System;
using LY.Report.Core.Model.User;

namespace LY.Report.Core.Repository.Base
{
    /// <summary>
    /// 数据
    /// </summary>
    public class Data
    {
        public AuthApi[] Apis { get; set; }
        public AuthApi[] ApiTree { get; set; }
        public AuthView[] ViewTree { get; set; }
        public AuthPermission[] PermissionTree { get; set; }
        public UserInfo[] Users { get; set; }
        public AuthRole[] Roles { get; set; }
        public AuthUserRole[] UserRoles { get; set; }
        public AuthRolePermission[] RolePermissions { get; set; }
        public SysTenant[] Tenants { get; set; }
        public AuthTenantPermission[] TenantPermissions { get; set; }
        public AuthPermissionApi[] PermissionApis { get; set; }
        public PersonnelPosition[] Positions { get; set; }
        public PersonnelOrganization[] OrganizationTree { get; set; }
        public PersonnelEmployee[] Employees { get; set; }
    }
}
