using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.System
{
    /// <summary>
    /// 地区表
    /// </summary>
    [Table(Name = "T_Sys_Region")]
    [Index("Idx_{tablename}_01", nameof(RegionId), true)]
    public class SysRegion : EntityTenantFull
    {
        /// <summary>
        /// 地区Id
        /// </summary>
        /// <returns></returns>
        [Description("地区Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public int RegionId { get; set; }

        /// <summary>
        /// 上级Id
        /// </summary>
        /// <returns></returns>
        [Description("上级Id")]
        public int ParentId { get; set; }

        /// <summary>
        /// 完整Id
        /// </summary>
        /// <returns></returns>
        [Description("完整Id")]
        [Column(StringLength = 200)]
        public string FullId { get; set; }

        /// <summary>
        /// 地区名称
        /// </summary>
        /// <returns></returns>
        [Description("地区名称")]
        [Column(StringLength = 200)]
        public string RegionName { get; set; }

        /// <summary>
        /// 省级简称
        /// </summary>
        [Description("省级简称")]
        [Column(StringLength = 200)]
        public string ShortName { get; set; }

        /// <summary>
        /// 名称拼音
        /// </summary>
        [Description("名称拼音")]
        [Column(StringLength = 200)]
        public string PinYin { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [Description("经度")]
        public double Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        [Description("纬度")]
        public double Latitude { get; set; }

        /// <summary>
        /// 地区级别：1省，2市，3区
        /// </summary>
        /// <returns></returns>
        [Description("地区级别")]
        public int Depth { get; set; }

        /// <summary>
        /// 地区排序
        /// </summary>
        /// <returns></returns>
        [Description("地区排序")]
        public int Sequence { get; set; }
    }
}
