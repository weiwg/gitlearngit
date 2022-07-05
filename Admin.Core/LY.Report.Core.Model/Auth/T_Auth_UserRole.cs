using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.User;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace LY.Report.Core.Model.Auth
{
    /// <summary>
    /// 用户角色
    /// </summary>
	[Table(Name = "T_Auth_UserRole")]
    [Index("idx_{tablename}_01", nameof(UserRoleId), true)]
    public class AuthUserRole: EntityTenantFull
    {
        /// <summary>
        /// 用户角色Id
        /// </summary>
        [Description("用户角色Id")]
        [Column(IsPrimary = true, Position =2, StringLength = 36, IsNullable = false)]
        public string UserRoleId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        public UserInfo User { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public string RoleId { get; set; }

		public AuthRole Role { get; set; }
    }
}
