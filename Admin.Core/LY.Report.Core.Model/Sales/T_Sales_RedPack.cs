using System;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Sales.Enum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Sales
{
    /// <summary>
    /// 红包
    /// </summary>
    [Table(Name = "T_Sales_RedPack")]
    [Index("idx_{tablename}_01", nameof(RedPackId), true)]
    public class SalesRedPack : EntityTenantFull
    {
        /// <summary>
        /// 红包Id
        /// </summary>
        [Description("红包Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string RedPackId { get; set; }

        /// <summary>
        /// 红包名称
        /// </summary>
        [Description("红包名称")]
        [Column(StringLength = 50, IsNullable = false)]
        public string RedPackName { get; set; }

        /// <summary>
        /// 红包金额
        /// </summary>
        [Description("红包金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal RedPackAmount { get; set; }

        /// <summary>
        /// 生效方式
        /// </summary>
        [Description("生效方式")]
        public RedPackEffectiveType EffectiveType { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        [Description("生效时间")]
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        [Description("失效时间")]
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// 有效时间(分钟)
        /// </summary>
        [Description("有效时间")]
        public int EffectiveTime { get; set; }

        /// <summary>
        /// 发行数量
        /// </summary>
        [Description("发行数量")]
        public int PublishCount { get; set; }

        /// <summary>
        /// 限领数量
        /// </summary>
        [Description(" 限领数量")]
        public int LimitCount { get; set; }

        /// <summary>
        /// 可领数量
        /// </summary>
        [Description("可领数量")]
        public int RemainCount { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        [Description("是否有效")]
        public IsActive IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        [Column(StringLength = 100)]
        public string Remark { get; set; }
    }
}
