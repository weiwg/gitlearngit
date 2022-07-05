
namespace LY.Report.Core.Service.System.Region.Output
{
    public class SysRegionGetOutput
    {
        /// <summary>
        /// 地区ID
        /// </summary>
        /// <returns></returns>
        public int RegionId { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        /// <returns></returns>
        public int ParentId { get; set; }

        /// <summary>
        /// 完整Id
        /// </summary>
        /// <returns></returns>
        public string FullId { get; set; }

        /// <summary>
        /// 地区名称
        /// </summary>
        /// <returns></returns>
        public string RegionName { get; set; }

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
        /// 地区级别：1省，2市，3区
        /// </summary>
        /// <returns></returns>
        public int Depth { get; set; }

        /// <summary>
        /// 地区排序
        /// </summary>
        /// <returns></returns>
        public int Sequence { get; set; }
    }
}
