using System;
using System.Collections.Generic;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Auth.Enum;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.User;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Auth
{
    /// <summary>
    /// 角色
    /// </summary>
	[Table(Name = "T_Auth_Role")]
    [Index("idx_{tablename}_01", nameof(RoleId) + "," + nameof(TenantId), true)]
    //[Index("idx_{tablename}_02", nameof(Code) + "," + nameof(TenantId), true)]
    public class AuthRole: EntityTenantFull
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [Description("角色Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string RoleId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public RoleType RoleType { get; set; } = RoleType.User;

        /// <summary>
        /// 说明
        /// </summary>
        [Column(StringLength = 200)]
		public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public IsActive IsActive { get; set; } = IsActive.Yes;

        /// <summary>
        /// 排序
        /// </summary>
		public int Sort { get; set; }

        [Navigate(ManyToMany = typeof(AuthUserRole))]
        public ICollection<UserInfo> Users { get; set; }

        [Navigate(ManyToMany = typeof(AuthRolePermission))]
        public ICollection<AuthPermission> Permissions { get; set; }
    }

}
