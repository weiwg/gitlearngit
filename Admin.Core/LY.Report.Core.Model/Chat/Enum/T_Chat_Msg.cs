using System.ComponentModel;

namespace LY.Report.Core.Model.Chat.Enum
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MsgType
    {
        /// <summary>
        /// 系统
        /// </summary>
        [Description("系统")]
        System = 1,
        /// <summary>
        /// 订单
        /// </summary>
        [Description("订单")]
        Order = 2
    }

    /// <summary>
    /// 消息状态
    /// </summary>
    public enum MsgStatus
    {
        /// <summary>
        /// 未读
        /// </summary>
        [Description("未读")]
        Unread = 1,
        /// <summary>
        /// 已读
        /// </summary>
        [Description("已读")]
        Read = 2
    }
}
