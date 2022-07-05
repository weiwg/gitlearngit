using System.ComponentModel;

namespace LY.Report.Core.Model.Mq.Enum
{
    /// <summary>
    /// 处理状态
    /// </summary>
    public enum MqMsgStatus
    {
        /// <summary>
        /// 未处理
        /// </summary>
        [Description("未处理")]
        NotHandled = 1,
        /// <summary>
        /// 已处理
        /// </summary>
        [Description("已发处理")]
        Handled = 2,
        /// <summary>
        /// 处理失败
        /// </summary>
        [Description("处理失败")]
        HandleFail = 3
    }
}
