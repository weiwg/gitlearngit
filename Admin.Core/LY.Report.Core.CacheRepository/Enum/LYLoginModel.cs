using System;
using System.ComponentModel;

namespace LY.Report.Core.CacheRepository.Enum
{
    /// <summary>
    /// EUA登录模式
    /// </summary>
    [Serializable]
    public enum LYLoginModel
    {
        /// <summary>
        /// 本地
        /// </summary>
        [Description("本地")]
        Local = 0,
        /// <summary>
        /// 单点
        /// </summary>
        [Description("单点")]
        SingleSign = 1,
        /// <summary>
        /// 共享
        /// </summary>
        [Description("共享")]
        StateServer = 2
    }
}
