using System;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Auth
{
    /// <summary>
    /// 权限接口
    /// </summary>
	[Table(Name = "T_Auth_PermissionApi")]
    [Index("idx_{tablename}_01", nameof(PermissionApiId), true)]
    public class AuthPermissionApi : EntityTenantFull
    {
        /// <summary>
        /// 角色权限Id
        /// </summary>
        [Description("角色权限Id")]
        [Column(IsPrimary = true, Position =2, StringLength = 36, IsNullable = false)]
        public string PermissionApiId { get; set; }
        /// <summary>
        /// 权限Id
        /// </summary>
		public string PermissionId { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public AuthPermission Permission { get; set; }

        /// <summary>
        /// 接口Id
        /// </summary>
		public string ApiId { get; set; }

        /// <summary>
        /// 接口
        /// </summary>
        public AuthApi Api { get; set; }
    }
}
