
using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Record.Location.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class RecordLocationAddInput
    {
        /// <summary>
        /// 坐标(经度,纬度)
        /// </summary>
        public string Coordinate { get; set; }
    }
}
