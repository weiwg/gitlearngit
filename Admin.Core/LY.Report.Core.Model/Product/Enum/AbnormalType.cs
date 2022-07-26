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
        /// 物料问题
        /// </summary>
        [Description("物料问题")]
        Material = 10,
        /// <summary>
        /// 设备问题
        /// </summary>
        [Description("设备问题")]
        Equipment = 11,
        /// <summary>
        /// 模具问题
        /// </summary>
        [Description("模具问题")]
        Mould = 12,
        /// <summary>
        /// 制程问题
        /// </summary>
        [Description("制程问题")]
        Process = 13,
        /// <summary>
        /// 测试问题
        /// </summary>
        [Description("测试问题")]
        Test = 14,
        /// <summary>
        /// SFC问题
        /// </summary>
        [Description("SFC问题")]
        SFC = 15,
        /// <summary>
        /// 电力问题
        /// </summary>
        [Description("电力问题")]
        Power = 16,
        /// <summary>
        /// 网络问题
        /// </summary>
        [Description("网络问题")]
        Network = 17,
        /// <summary>
        /// 压缩气问题
        /// </summary>
        [Description("压缩气问题")]
        CompressedGas = 18,
        /// <summary>
        /// 其他问题
        /// </summary>
        [Description("其他问题")]
        Other = 19
    }
}
