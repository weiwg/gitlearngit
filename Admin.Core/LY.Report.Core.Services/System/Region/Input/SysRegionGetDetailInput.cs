
namespace LY.Report.Core.Service.System.Region.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class SysRegionGetDetailInput
    {
        /// <summary>
        /// 省份ID
        /// </summary>
        /// <returns></returns>
        public int ProvinceId { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        /// <returns></returns>
        public string ProvinceName { get; set; }

        /// <summary>
        /// 城市ID
        /// </summary>
        /// <returns></returns>
        public int CityId { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        /// <returns></returns>
        public string CityName { get; set; }

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
