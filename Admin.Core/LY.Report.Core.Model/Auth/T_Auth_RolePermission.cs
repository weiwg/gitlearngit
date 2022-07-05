using LY.Report.Core.Common.BaseModel;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace LY.Report.Core.Model.Auth
{
    /// <summary>
    /// 角色权限
    /// </summary>
	[Table(Name = "T_Auth_RolePermission")]
    [Index("idx_{tablename}_01", nameof(RolePermissionId), true)]
    public class AuthRolePermission: EntityTenantFull
    {
        /// <summary>
        /// 角色权限Id
        /// </summary>
        [Description("角色权限Id")]
        [Column(IsPrimary = true, Position = 2,StringLength = 36, IsNullable = false)]
        public string RolePermissionId { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
		public string RoleId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
		public string PermissionId { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public AuthRole Role { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public AuthPermission Permission { get; set; }
    }

}
