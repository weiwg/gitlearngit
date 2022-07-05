using System;
using System.ComponentModel;

namespace LY.Report.Core.Model.BaseEnum
{
    /// <summary>
    /// 删除标记
    /// </summary>
    [Serializable]
    public enum Del
    {
        /// <summary>
        /// 不删除
        /// </summary>
        [Description("不删除")]
        No = 0,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Yes = 1
    }

    /// <summary>
    /// 是否有效
    /// </summary>
    [Serializable]
    public enum IsActive
    {
        /// <summary>
        /// 有效
        /// </summary>
        [Description("有效")]
        Yes = 1,
        /// <summary>
        /// 无效
        /// </summary>
        [Description("无效")]
        No = 2
    }

    /// <summary>
    /// 是否默认
    /// </summary>
    [Serializable]
    public enum IsDefault
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("默认")]
        Yes = 1,
        /// <summary>
        /// 非默认
        /// </summary>
        [Description("非默认")]
        No = 2,
    }

}
