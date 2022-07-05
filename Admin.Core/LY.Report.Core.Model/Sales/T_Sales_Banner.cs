using System;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Sales.Enum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Sales
{
    /// <summary>
    /// 横幅配置
    /// </summary>
    [Table(Name = "T_Sales_Banner")]
    [Index("idx_{tablename}_01", nameof(BannerId), true)]
    public class SalesBanner : EntityTenantFull
    {
        /// <summary>
        /// 横幅Id
        /// </summary>
        [Description("横幅Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string BannerId { get; set; }

        /// <summary>
        /// 横幅名称
        /// </summary>
        [Description("横幅名称")]
        [Column(StringLength = 50, IsNullable = false)]
        public string BannerName { get; set; }

        /// <summary>
        /// 横幅类型
        /// </summary>
        [Description("横幅类型")]
        [Column(StringLength = 50, IsNullable = false)]
        public BannerType BannerType { get; set; }

        /// <summary>
        /// 地区Id(全国,省,市)
        /// </summary>
        [Description("地区Id")]
        [Column(IsNullable = false)]
        public int RegionId { get; set; }

        /// <summary>
        /// 横幅图片
        /// </summary>
        [Description("横幅图片")]
        [Column(StringLength = 100, IsNullable = false)]
        public string BannerImg { get; set; }

        /// <summary>
        /// 横幅链接
        /// </summary>
        [Description("横幅链接")]
        [Column(StringLength = -1)]
        public string BannerLink { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Description("开始时间")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Description("结束时间")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 点击次数
        /// </summary>
        [Description("点击次数")]
        public long ClickCount { get; set; }

        /// <summary>
        /// 排序(越大越靠前)
        /// </summary>
        [Description("排序")]
        [Column(StringLength = 100)]
        public int Sequence { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        [Description("是否有效")]
        [Column(IsNullable = false)]
        public IsActive IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        [Column(StringLength = 100)]
        public string Remark { get; set; }
    }
}