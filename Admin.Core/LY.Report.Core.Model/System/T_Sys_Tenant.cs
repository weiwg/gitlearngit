using System;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Common.BaseModel.Enum;
using LY.Report.Core.Model.Auth;
using LY.Report.Core.Model.BaseEnum;
using FreeSql;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.System
{
    /// <summary>
    /// 租户
    /// </summary>
	[Table(Name = "T_Sys_Tenant")]
    [Index("idx_{tablename}_01", nameof(TenantId), true)]
    public class SysTenant : EntityFull
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Description("租户Id")]
        [Column(IsPrimary = true, Position = 2,StringLength = 36, IsNullable = false)]
        public string TenantId { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [Column(StringLength = 50)]
        public string Code { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Column(StringLength = 50)]
        public string RealName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [Column(StringLength = 20)]
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        [Column(StringLength = 50)]
        public string Email { get; set; }

        /// <summary>
        /// 授权用户
        /// </summary>
        public string UserId { get; set; }

        //public UserEntity User { get; set; }

        /// <summary>
        /// 授权角色
        /// </summary>
        public string RoleId { get; set; }

        public AuthRole Role { get; set; }

        /// <summary>
        /// 租户类型
        /// </summary>
        public TenantType? TenantType { get; set; } = LY.Report.Core.Common.BaseModel.Enum.TenantType.Tenant;

        /// <summary>
        /// 数据隔离类型
        /// </summary>
        public DataIsolationType DataIsolationType { get; set; } = DataIsolationType.OwnDb;

        /// <summary>
        /// 数据库
        /// </summary>
        [Column(MapType = typeof(int))]
        public DataType? DbType { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        [Column(StringLength = 500)]
        public string ConnectionString { get; set; }

        /// <summary>
        /// 空闲时间(分)
        /// </summary>
        public int? IdleTime { get; set; } = 10;

        /// <summary>
        /// 启用
        /// </summary>
		public IsActive IsActive { get; set; } = IsActive.Yes;

        /// <summary>
        /// 说明
        /// </summary>
        [Column(StringLength = 500)]
        public string Description { get; set; }
    }

}
