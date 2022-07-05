using System.ComponentModel;

namespace LY.Report.Core.Model.Chat.Enum
{
    /// <summary>
    /// 聊天室类型
    /// </summary>
    public enum RoomType
    {
        /// <summary>
        /// 私聊
        /// </summary>
        [Description("私聊")]
        PrivateChat = 10,
        /// <summary>
        /// 群聊
        /// </summary>
        [Description("群聊")]
        GroupChat = 11
    }

}
