using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Mq.SendRecord.Input
{
    /// <summary>
    /// 删除
    /// </summary>
    public class MqSendRecordDeleteInput
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        [Required(ErrorMessage = "记录Id")]
        public string RecordId { get; set; }
    }
}
