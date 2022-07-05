using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Chat.Enum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Chat
{
    /// <summary>
    /// 消息表
    /// </summary>
    [Table(Name = "T_Chat_Msg")]
    [Index("idx_{tablename}_01", nameof(MsgId), true)]
    public class ChatMsg : EntityTenantFull
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        [Description("消息Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string MsgId { get; set; }

        /// <summary>
        /// 消息详情Id
        /// </summary>
        [Description("消息详情Id")]
        [Column(StringLength = 50, IsNullable = false)]
        public string MsgDetailId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Description("用户Id")]
        [Column(StringLength = 36, IsNullable = false)]
        public string UserId { get; set; }

        /// <summary>
        /// 消息状态
        /// </summary>
        [Description("消息状态")]
        public MsgStatus MsgStatus { get; set; }

    }
}
