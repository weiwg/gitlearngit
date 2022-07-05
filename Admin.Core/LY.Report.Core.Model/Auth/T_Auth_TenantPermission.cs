using System;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.System;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Auth
{
    /// <summary>
    /// 租户权限
    /// </summary>
	[Table(Name = "T_Auth_TenantPermission")]
    [Index("idx_{tablename}_01", nameof(TenantPermissionId), true)]
    public class AuthTenantPermission : EntityTenantFull
    {
        /// <summary>
        /// 租户权限Id
        /// </summary>
        [Description("租户权限Id")]
        [Column(IsPrimary = true, Position = 2, StringLength = 36, IsNullable = false)]
        public string TenantPermissionId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
		public string PermissionId { get; set; }

        /// <summary>
        /// 租户
        /// </summary>
        public SysTenant Tenant { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public AuthPermission Permission { get; set; }
    }
}
