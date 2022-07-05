
using System.ComponentModel;

namespace LY.Report.Core.Model.Delivery.Enum
{
    /// <summary>
    /// 计价类型
    /// </summary>
    public enum CalcRuleType
    {
        /// <summary>
        /// 距离km
        /// </summary>
        [Description("距离")]
        Distance = 1,
        /// <summary>
        /// 重量kg
        /// </summary>
        [Description("重量")]
        Weight = 2,
        /// <summary>
        /// 体积m³
        /// </summary>
        [Description("体积")]
        Volume = 3,
        /// <summary>
        /// 面积㎡
        /// </summary>
        [Description("面积")]
        Area = 4
    }

}
