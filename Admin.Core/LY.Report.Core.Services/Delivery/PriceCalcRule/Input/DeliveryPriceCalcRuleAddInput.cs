

using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Delivery.Enum;

namespace LY.Report.Core.Service.Delivery.PriceCalcRule.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class DeliveryPriceCalcRuleAddInput
    {
        /// <summary>
        /// 车型Id
        /// </summary>
        [Required(ErrorMessage = "车型不能为空")]
        public string CarId { get; set; }

        /// <summary>
        /// 地区Id(全国,省,市)
        /// </summary>
        [Required(ErrorMessage = "地区Id不能为空")]
        public int RegionId { get; set; }

        /// <summary>
        /// 计价类型
        /// </summary>
        [Required(ErrorMessage = "计价类型不能为空")]
        public CalcRuleType CalcRuleType { get; set; }

        /// <summary>
        /// 条件 距离km/体积m³/面积㎡/重量kg
        /// </summary>
        [Required(ErrorMessage = "条件不能为空")]
        public double Condition { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        [Required(ErrorMessage = "运费不能为空")]
        public decimal Freight { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        [Required(ErrorMessage = "是否有效不能为空")]
        public IsActive IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [StringLength(100, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 0)]
        public string Remark { get; set; }
    }
}
