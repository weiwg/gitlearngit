using System;

namespace LY.Report.Core.Service.Record.Location.Output
{
    public class RecordLocationGetOutput
    {
        /// <summary>
        /// 坐标(经度,纬度)
        /// </summary>
        public string Coordinate { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime RecordDate { get; set; }
    }
}
