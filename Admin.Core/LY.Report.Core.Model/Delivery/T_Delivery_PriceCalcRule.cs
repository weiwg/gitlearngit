using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Delivery.Enum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Delivery
{
    /// <summary>
    /// 计价规则
    /// </summary>
    [Table(Name = "T_Delivery_PriceCalcRule")]
    [Index("idx_{tablename}_01", nameof(PriceRuleId), true)]
    public class DeliveryPriceCalcRule : EntityTenantFull
    {
        /// <summary>
        /// 规则Id
        /// </summary>
        [Description("规则Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string PriceRuleId { get; set; }

        /// <summary>
        /// 地区Id(全国,省,市)
        /// </summary>
        [Description("地区Id")]
        [Column(IsNullable = false)]
        public int RegionId { get; set; }

        /// <summary>
        /// 车型Id
        /// </summary>
        [Description("车型Id")]
        [Column(StringLength = 50, IsNullable = false)]
        public string CarId { get; set; }

        /// <summary>
        /// 计价类型
        /// </summary>
        [Description("计价类型")]
        [Column(IsNullable = false)]
        public CalcRuleType CalcRuleType { get; set; }

        /// <summary>
        /// 条件 距离/体积/面积
        /// </summary>
        [Description("条件")]
        [Column(StringLength = 50, IsNullable = false)]
        public double Condition { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        [Description("运费")]
        [Column(Precision = 12, Scale = 2)]
        public decimal Freight { get; set; }

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