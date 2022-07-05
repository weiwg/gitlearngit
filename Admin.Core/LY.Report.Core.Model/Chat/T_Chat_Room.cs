using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Chat.Enum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Chat
{
    /// <summary>
    /// 聊天室
    /// </summary>
    [Table(Name = "T_Chat_Room")]
    [Index("idx_{tablename}_01", nameof(RoomId), true)]
    public class ChatRoom: EntityTenantFull
    {
        /// <summary>
        /// 聊天室Id
        /// </summary>
        [Description("聊天室Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string RoomId { get; set; }

        /// <summary>
        /// 聊天室名称
        /// </summary>
        [Description("聊天室名称")]
        [Column(StringLength = 50, IsNullable = false)]
        public string RoomName { get; set; }

        /// <summary>
        /// 聊天室类型
        /// </summary>
        [Description("聊天室类型")]
        [Column(IsNullable = false)]
        public RoomType RoomType { get; set; }

        /// <summary>
        /// 聊天室管理员用户Id
        /// </summary>
        [Description("聊天室管理员用户Id")]
        [Column(StringLength = 36, IsNullable = false)]
        public string AdminUserId { get; set; }
      
    }
}
