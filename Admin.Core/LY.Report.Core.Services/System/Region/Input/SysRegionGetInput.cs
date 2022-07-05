using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.System;

namespace LY.Report.Core.Service.System.Region.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class SysRegionGetInput
    {
        /// <summary>
        /// 地区ID
        /// </summary>
        /// <returns></returns>
        public int RegionId { get; set; }

        /// <summary>
        /// 地区名称
        /// </summary>
        /// <returns></returns>
        public string RegionName { get; set; }
    }
}
