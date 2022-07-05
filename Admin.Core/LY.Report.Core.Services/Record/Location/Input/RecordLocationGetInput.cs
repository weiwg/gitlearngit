
using System;

namespace LY.Report.Core.Service.Record.Location.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class RecordLocationGetInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 司机Id
        /// </summary>
        public string DriverId { get; set; }
    }
}
