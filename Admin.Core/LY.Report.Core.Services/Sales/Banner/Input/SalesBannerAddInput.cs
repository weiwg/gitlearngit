using System;
using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Sales.Enum;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Service.Sales.Banner.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class SalesBannerAddInput
    {
        /// <summary>
        /// 横幅名称
        /// </summary>
        [Display(Name = "横幅名称")]
        [Required(ErrorMessage = "横幅名称不能为空"), StringLength(50, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string BannerName { get; set; }

        /// <summary>
        /// 横幅类型
        /// </summary>
        [Required(ErrorMessage = "横幅类型不能为空")]
        public BannerType BannerType { get; set; }

        /// <summary>
        /// 地区Id(全国,省,市)
        /// </summary>
        [Required(ErrorMessage = "地区Id不能为空")]
        public int RegionId { get; set; }

        /// <summary>
        /// 横幅图片
        /// </summary>
        [Required(ErrorMessage = "横幅图片不能为空！")]
        private string _bannerImg;
        public string BannerImg { get => _bannerImg; set => _bannerImg = EncryptHelper.Aes.Decrypt(value); }

        /// <summary>
        /// 横幅链接
        /// </summary>
        //[Required(ErrorMessage = "横幅链接不能为空")]
        public string BannerLink { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Required(ErrorMessage = "开始时间不能为空")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Required(ErrorMessage = "结束时间不能为空")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 排序(越大越靠前)
        /// </summary>
        [Required(ErrorMessage = "排序不能为空")]
        public int Sequence { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        [Required(ErrorMessage = "是否有效不能为空")]
        public IsActive IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [StringLength(100, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 0)]
        public string Remark { get; set; }
    }
}
