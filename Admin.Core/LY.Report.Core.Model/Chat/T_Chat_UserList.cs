using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Chat
{
    /// <summary>
    /// 用户聊天列表
    /// </summary>
    [Table(Name = "T_Chat_UserList")]
    [Index("idx_{tablename}_01", nameof(ListId), true)]
    public class ChatUserList : EntityTenantFull
    {
        /// <summary>
        /// 列表Id
        /// </summary>
        [Description("聊天室用户Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string ListId { get; set; }

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
    }
}
