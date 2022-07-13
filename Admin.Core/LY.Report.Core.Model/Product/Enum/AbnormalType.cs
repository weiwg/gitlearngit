using System.ComponentModel;

namespace LY.Report.Core.Model.Product.Enum
{
    /// <summary>
    /// 异常大类(异常/停线)
    /// </summary>
    public enum AbnormalType
    {
        /// <summary>
        /// 异常
        /// </summary>
        [Description("异常")]
        Abnormal = 1,
        /// <summary>
        /// 停线
        /// </summary>
        [Description("停线")]
        StopLine = 2
    }

    /// <summary>
    /// 小类(机器故障/物料异常/停电等)
    /// </summary>
    public enum AbnormalItemType
    {
        /// <summary>
        /// 机械故障
        /// </summary>
        [Description("机械故障")]
        MechanicalFailure = 10,
        /// <summary>
        /// 物料异常
        /// </summary>
        [Description("物料异常")]
        MaterialAbnormality = 11,
        /// <summary>
        /// 停电
        /// </summary>
        [Description("停电")]
        PowerFailure = 12
    }
}
