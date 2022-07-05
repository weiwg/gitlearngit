using System;

namespace LY.Report.Core.Service.Driver.IdentityInfo.Output
{
    public class DriverIdentityInfoListOutput : DriverIdentityInfoGetOutput
    {
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
