using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Chat.Enum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Chat
{
    /// <summary>
    /// 聊天消息表
    /// </summary>
    [Table(Name = "T_Chat_RoomMsg")]
    [Index("idx_{tablename}_01", nameof(RoomMsgId), true)]
    public class ChatRoomMsg: EntityTenantFull
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        [Description("消息Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string RoomMsgId { get; set; }

        /// <summary>
        /// 聊天室Id
        /// </summary>
        [Description("消息Id")]
        [Column(StringLength = 36, IsNullable = false)]
        public string RoomId { get; set; }

        /// <summary>
        /// 发送用户Id
        /// </summary>
        [Description("发送用户Id")]
        [Column( StringLength = 36, IsNullable = false)]
        public string FromUserId { get; set; }

        /// <summary>
        /// 聊天内容
        /// </summary>
        [Description("聊天内容")]
        [Column(StringLength = -1, IsNullable = false)]
        public string ChatContent { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        [Description("消息类型")]
        [Column(IsNullable = false)]
        public ChatMsgType ChatMsgType { get; set; }
    }
}
