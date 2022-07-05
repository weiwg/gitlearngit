using System;

namespace LY.Report.Core.Service.Driver.Info.Output
{
    public class DriverInfoListOutput : DriverInfoGetOutput
    {
        /// <summary>
        /// 司机Id
        /// </summary>
        public string DriverId { get; set; }

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
