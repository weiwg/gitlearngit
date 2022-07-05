using System;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.BaseEnum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Personnel
{
    /// <summary>
    /// 职位
    /// </summary>
	[Table(Name = "T_Personnel_Position")]
    [Index("idx_{tablename}_01", nameof(PositionId), true)]
    public class PersonnelPosition : EntityFull, ITenant
    {
        /// <summary>
        /// 职位Id
        /// </summary>
        [Description("职位Id")]
        [Column(IsPrimary = true, Position = 2, StringLength = 36, IsNullable = false)]
        public string PositionId { get; set; }
        /// <summary>
        /// 租户Id
        /// </summary>
        [Column(Position = -10, CanUpdate = false)]
        public string TenantId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Column(StringLength = 50)]
        public string Code { get; set; }

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
    }
}
