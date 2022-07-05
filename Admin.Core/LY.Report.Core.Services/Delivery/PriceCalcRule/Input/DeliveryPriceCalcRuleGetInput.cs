using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Delivery.Enum;

namespace LY.Report.Core.Service.Delivery.PriceCalcRule.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class DeliveryPriceCalcRuleGetInput
    {
        /// <summary>
        /// 规则Id
        /// </summary>
        public string PriceRuleId { get; set; }
        /// <summary>
        /// 车型Id
        /// </summary>
        public string CarId { get; set; }

        /// <summary>
        /// 车型名称
        /// </summary>
        public string CarName { get; set; }

        /// <summary>
        /// 计价类型
        /// </summary>
        public CalcRuleType CalcRuleType { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public IsActive IsActive { get; set; }

        /// <summary>
        /// 地区
        /// </summary>
        public string RegionName { get; set; }

    }
}
