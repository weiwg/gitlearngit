using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.BaseEnum;
using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace LY.Report.Core.Model.Auth
{
    /// <summary>
    /// 视图管理
    /// </summary>
	[Table(Name = "T_Auth_View")]
    [Index("idx_{tablename}_01", nameof(ViewId), true)]
    public class AuthView : EntityTenantFull
    {
        /// <summary>
        /// 视图Id
        /// </summary>
        [Description("视图Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string ViewId { get; set; }
        /// <summary>
        /// 所属节点
        /// </summary>
		public string ParentId { get; set; }

        [Navigate(nameof(ParentId))]
        public List<AuthView> Childs { get; set; }

        /// <summary>
        /// 视图命名
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 视图名称
        /// </summary>
        [Column(StringLength = 500)]
        public string Label { get; set; }

        /// <summary>
        /// 视图路径
        /// </summary>
        [Column(StringLength = 500)]
        public string Path { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Column(StringLength = 500)]
        public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
        public IsActive IsActive { get; set; } = IsActive.Yes;

        /// <summary>
        /// 缓存
        /// </summary>
        public bool Cache { get; set; } = true;

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// Api版本号
        /// </summary>
        [Description("Api版本号")]
        [Column(IsNullable = false, StringLength = 100)]
        public string ApiVersion { get; set; } = "V0";
    }
}
