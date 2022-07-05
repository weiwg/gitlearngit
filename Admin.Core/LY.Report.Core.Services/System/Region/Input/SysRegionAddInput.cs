using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.System.Region.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class SysRegionAddInput
    {
        /// <summary>
        /// 地区ID
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "请输入地区ID")]
        public int RegionId { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "请输入上级ID")]
        public int ParentId { get; set; }

        ///// <summary>
        ///// 完整Id
        ///// </summary>
        ///// <returns></returns>
        //[Display(Name = "完整Id")]
        //[StringLength(200, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 1)]
        //[Required(ErrorMessage = "请输入完整Id")]
        //public string FullId { get; set; }

        /// <summary>
        /// 地区名称
        /// </summary>
        /// <returns></returns>
        [Display(Name = "地区名称")]
        [StringLength(200, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        [Required(ErrorMessage = "请输入地区名称")]
        public string RegionName { get; set; }

        /// <summary>
        /// 省级简称
        /// </summary>
        [Display(Name = "省级简称")]
        [StringLength(200, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        [Required(ErrorMessage = "请输入省级简称")]
        public string ShortName { get; set; }

        /// <summary>
        /// 名称拼音
        /// </summary>
        [Display(Name = "名称拼音")]
        [StringLength(200, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        [Required(ErrorMessage = "请输入名称拼音")]
        public string PinYin { get; set; }

        ///// <summary>
        ///// 经度
        ///// </summary>
        //[Required(ErrorMessage = "请输入经度")]
        //public double Longitude { get; set; }

        ///// <summary>
        ///// 纬度
        ///// </summary>
        //[Required(ErrorMessage = "请输入纬度")]
        //public double Latitude { get; set; }

        ///// <summary>
        ///// 地区级别：1省，2市，3区
        ///// </summary>
        ///// <returns></returns>
        //[Required(ErrorMessage = "请输入地区级别")]
        //public int Depth { get; set; }

        /// <summary>
        /// 地区排序
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "请输入地区排序")]
        public int Sequence { get; set; }
    }
}
