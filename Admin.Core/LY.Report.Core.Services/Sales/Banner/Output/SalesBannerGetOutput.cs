

using System;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Sales.Enum;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Service.Sales.Banner.Output
{
    public class SalesBannerGetOutput
    {
        /// <summary>
        /// 横幅Id
        /// </summary>
        public string BannerId { get; set; }

        /// <summary>
        /// 横幅名称
        /// </summary>
        public string BannerName { get; set; }

        /// <summary>
        /// 横幅类型
        /// </summary>
        public BannerType BannerType { get; set; }

        /// <summary>
        /// 横幅类型描述
        /// </summary>
        public string BannerTypeDescribe => EnumHelper.GetDescription(BannerType);

        /// <summary>
        /// 地区Id(全国,省,市)
        /// </summary>
        public int RegionId { get; set; }
        /// <summary>
        /// 完整父级Id
        /// </summary>
        /// <returns></returns>
        public string RegionFullId { get; set; }
        /// <summary>
        /// 横幅图片
        /// </summary>
        private string _bannerImg;
        public string BannerImg { get => _bannerImg.IsNull() ? "" : EncryptHelper.Aes.Encrypt(_bannerImg); set => _bannerImg = value; }

        /// <summary>
        /// 横幅图片Url
        /// </summary>
        public string BannerImgUrl { get => _bannerImg; }

        /// <summary>
        /// 横幅链接
        /// </summary>
        public string BannerLink { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 点击次数
        /// </summary>
        public long ClickCount { get; set; }

        /// <summary>
        /// 排序(越大越靠前)
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public IsActive IsActive { get; set; }
        /// <summary>
        /// 地区名称
        /// </summary>
        public string RegionName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
