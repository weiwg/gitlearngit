
using System;

namespace LY.Report.Core.Service.Record.Login.Output
{
    public class RecordLoginListOutput : RecordLoginGetOutput
    {
        /// <summary>
        /// 日志Id
        /// </summary>
        public string LogId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateDate { get; set; }
    }
}
