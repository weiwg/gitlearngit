using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.Mq.Enum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Mq
{
    /// <summary>
    /// 消息接收记录
    /// </summary>
    [Table(Name = "T_Mq_ReceiveRecord")]
    [Index("idx_{tablename}_01", nameof(RecordId), true)]
    public class MqReceiveRecord : BaseMqMessage
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        [Description("记录Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string RecordId { get; set; }

        /// <summary>
        /// 消息状态
        /// </summary>
        [Description("消息状态")]
        [Column(IsNullable = false)]
        public MqMsgStatus MsgStatus { get; set; }

        /// <summary>
        /// 消息结果
        /// </summary>
        [Description("消息结果")]
        [Column(StringLength = -1)]
        public string MsgResult { get; set; }

        /// <summary>
        /// 回调消息状态
        /// </summary>
        [Description("回调消息状态")]
        [Column(IsNullable = false)]
        public MqMsgStatus MsgCallBackStatus { get; set; }

        /// <summary>
        /// 回调消息时间
        /// </summary>
        [Display(Name = "回调消息时间")]
        public DateTime? MsgCallBackDate { get; set; }
    }
}
