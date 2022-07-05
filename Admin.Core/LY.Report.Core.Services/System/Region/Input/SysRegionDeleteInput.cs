using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.System.Region.Input
{
    /// <summary>
    /// 删除
    /// </summary>
    public class SysRegionDeleteInput
    {
        /// <summary>
        /// 地区ID
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "请输入地区ID")]
        public int RegionId { get; set; }
    }
}
