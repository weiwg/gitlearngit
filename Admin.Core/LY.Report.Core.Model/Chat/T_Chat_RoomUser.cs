using System;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Chat
{
    /// <summary>
    /// 聊天室用户关联表
    /// </summary>
    [Table(Name = "T_Chat_RoomUser")]
    [Index("idx_{tablename}_01", nameof(RoomUserId), true)]
    public class ChatRoomUser: EntityTenantFull
    {
        /// <summary>
        /// 聊天室用户Id
        /// </summary>
        [Description("聊天室用户Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string RoomUserId { get; set; }

        /// <summary>
        /// 聊天室Id
        /// </summary>
        [Description("聊天室Id")]
        [Column(StringLength = 36, IsNullable = false)]
        public string RoomId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Description("用户Id")]
        [Column(StringLength = 36, IsNullable = false)]
        public string UserId { get; set; }

        /// <summary>
        /// 最后读取消息时间
        /// </summary>
        [Description("最后读取消息时间")]
        public DateTime? LastReadMsgDate { get; set; }
    }
}
