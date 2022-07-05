using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Chat.Enum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Chat
{
    /// <summary>
    /// 消息详情表
    /// </summary>
    [Table(Name = "T_Chat_MsgDetail")]
    [Index("idx_{tablename}_01", nameof(MsgDetailId), true)]
    public class ChatMsgDetail : EntityTenantFull
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        [Description("消息Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string MsgDetailId { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        [Description("消息类型")]
        public MsgType MsgType { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        [Description("消息标题")]
        [Column(StringLength = 50, IsNullable = false)]
        public string MsgTitle { get; set; }
        
        /// <summary>
        /// 消息封面图片
        /// </summary>
        [Description("消息封面图片")]
        [Column(StringLength = 100)]
        public string MsgCover { get; set; }
        
        /// <summary>
        /// 消息内容
        /// </summary>
        [Description("消息内容")]
        [Column(StringLength = -1, IsNullable = false)]
        public string MsgContent { get; set; }

        /// <summary>
        /// 消息链接
        /// </summary>
        [Description("消息链接")]
        [Column(StringLength = -1)]
        public string MsgUrl { get; set; }
    }
}
