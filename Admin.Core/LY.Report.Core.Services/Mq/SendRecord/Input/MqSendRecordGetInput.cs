using LY.Report.Core.Model.Mq.Enum;

namespace LY.Report.Core.Service.Mq.SendRecord.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class MqSendRecordGetInput
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        public string MsgId { get; set; }

        /// <summary>
        /// 消息模块
        /// </summary>
        public string MsgAction { get; set; }

        /// <summary>
        /// 消息状态
        /// </summary>
        public MqMsgStatus MsgStatus { get; set; }

    }
}
