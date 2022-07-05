using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.BaseEnum;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace LY.Report.Core.Model.Auth
{
    /// <summary>
    /// 接口管理
    /// </summary>
	[Table(Name = "T_Auth_Api")]
    [Index("idx_{tablename}_01", nameof(ApiId), true)]
    public class AuthApi : EntityTenantFull
    {
        /// <summary>
        /// ApiId
        /// </summary>
        [Description("ApiId")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string ApiId { get; set; }
        /// <summary>
        /// 所属模块
        /// </summary>
		public string ParentId { get; set; }

        [Navigate(nameof(ParentId))]
        public List<AuthApi> Childs { get; set; }

        /// <summary>
        /// 接口命名
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        [Column(StringLength = 500)]
        public string Label { get; set; }

        /// <summary>
        /// 接口地址
        /// </summary>
        [Column(StringLength = 500)]
        public string Path { get; set; }

        /// <summary>
        /// 接口提交方法
        /// </summary>
        [Column(StringLength = 50)]
        public string HttpMethods { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Column(StringLength = 500)]
        public string Description { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public IsActive IsActive { get; set; } = IsActive.Yes;


        [Navigate(ManyToMany = typeof(AuthPermissionApi))]
        public ICollection<AuthPermission> Permissions { get; set; }

        /// <summary>
        /// Api版本号
        /// </summary>
        [Description("Api版本号")]
        [Column(IsNullable = false, StringLength = 100)]
        public string ApiVersion { get; set; } = "V0";

    }
}
