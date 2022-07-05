using LY.Report.Core.Service.System.Region.Input;

namespace LY.Report.Core.Service.System.Region.Output
{
    public class SysRegionGetDetailOutput 
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
        /// 地区级别：1省，2市，3区
        /// </summary>
        /// <returns></returns>
        public int ProvinceDepth { get; set; }

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
        /// 地区级别：1省，2市，3区
        /// </summary>
        /// <returns></returns>
        public int CityDepth { get; set; }

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

        /// <summary>
        /// 地区级别：1省，2市，3区
        /// </summary>
        /// <returns></returns>
        public int RegionDepth { get; set; }

        /// <summary>
        /// 完整Id
        /// </summary>
        /// <returns></returns>
        public string FullId { get; set; }

        /// <summary>
        /// 省级简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 名称拼音
        /// </summary>
        public string PinYin { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
        
        /// <summary>
        /// 地区排序
        /// </summary>
        /// <returns></returns>
        public int Sequence { get; set; }
    }
}
