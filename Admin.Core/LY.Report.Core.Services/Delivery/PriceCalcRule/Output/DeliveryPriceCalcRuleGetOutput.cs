

using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Delivery.Enum;

namespace LY.Report.Core.Service.Delivery.PriceCalcRule.Output
{
    public class DeliveryPriceCalcRuleGetOutput
    {
        /// <summary>
        /// 车型Id
        /// </summary>
        public string CarId { get; set; }

        /// <summary>
        /// 车型名称
        /// </summary>
        public string CarName { get; set; }

        /// <summary>
        /// 地区Id(全国,省,市)
        /// </summary>
        public int RegionId { get; set; }
        
        /// <summary>
        /// 完整父级Id
        /// </summary>
        /// <returns></returns>
        public string RegionFullId { get; set; }

        /// <summary>
        /// 地区名称
        /// </summary>
        /// <returns></returns>
        public string RegionName { get; set; }

        /// <summary>
        /// 计价类型
        /// </summary>
        public CalcRuleType CalcRuleType { get; set; }

        /// <summary>
        /// 条件 距离/体积/面积
        /// </summary>
        public double Condition { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public decimal Freight { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public IsActive IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
