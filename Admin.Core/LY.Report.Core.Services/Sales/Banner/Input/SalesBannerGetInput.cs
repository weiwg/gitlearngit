using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Sales.Enum;

namespace LY.Report.Core.Service.Sales.Banner.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class SalesBannerGetInput
    {
        /// <summary>
        /// 横幅Id
        /// </summary>
        public string BannerId { get; set; }

        /// <summary>
        /// 横幅类型
        /// </summary>
        public BannerType BannerType { get; set; }

        /// <summary>
        /// 横幅名称
        /// </summary>
        public string BannerName { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public IsActive IsActive { get; set; }
    }
}
