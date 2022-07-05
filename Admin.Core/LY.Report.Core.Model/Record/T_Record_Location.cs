using System;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Record
{
    /// <summary>
    /// 定位表
    /// </summary>
	[Table(Name = "T_Record_Location")]
    [Index("idx_{tablename}_01", nameof(LocationId), true)]
    public class RecordLocation : EntityTenantFull
    {
        /// <summary>
        /// 坐标Id
        /// </summary>
        [Description("坐标Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string LocationId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Description("用户Id")]
        [Column(StringLength = 36, IsNullable = false)]
        public string UserId { get; set; }

        /// <summary>
        /// 坐标(经度,纬度)
        /// </summary>
        [Description("坐标")]
        [Column(StringLength = 100, IsNullable = false)]
        public string Coordinate { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        [Description("记录时间")]
        [Column(IsNullable = false)]
        public DateTime RecordDate { get; set; }
    }
}
