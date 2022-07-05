using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Mq.ReceiveRecord.Input
{
    /// <summary>
    /// 删除
    /// </summary>
    public class MqReceiveRecordDeleteInput
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        [Required(ErrorMessage = "记录Id")]
        public string RecordId { get; set; }
    }
}
