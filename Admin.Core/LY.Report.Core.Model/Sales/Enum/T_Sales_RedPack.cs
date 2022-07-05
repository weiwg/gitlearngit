using System.ComponentModel;

namespace LY.Report.Core.Model.Sales.Enum
{
    /// <summary>
    /// 生效方式
    /// </summary>
    public enum RedPackEffectiveType
    {
        /// <summary>
        /// 时间范围
        /// </summary>
        [Description("时间范围")]
        TimeRange = 1,
        /// <summary>
        /// 领取时间(分钟)
        /// </summary>
        [Description("领取时间")]
        ReceiveTime = 2,
    }

}
