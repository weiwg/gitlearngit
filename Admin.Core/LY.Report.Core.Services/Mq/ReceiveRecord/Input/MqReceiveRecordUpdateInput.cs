using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.Mq.Enum;

namespace LY.Report.Core.Service.Mq.ReceiveRecord.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class MqReceiveRecordUpdateInput
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        [Required(ErrorMessage = "记录Id")]
        public string RecordId { get; set; }

        /// <summary>
        /// 消息状态
        /// </summary>
        [Required(ErrorMessage = "消息状态")]
        public MqMsgStatus MsgStatus { get; set; }

        /// <summary>
        /// 消息结果
        /// </summary>
        [Required(ErrorMessage = "消息结果")]
        public string MsgResult { get; set; }
    }
}
