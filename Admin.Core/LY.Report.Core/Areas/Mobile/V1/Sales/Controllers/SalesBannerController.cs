using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Sales.Enum;
using LY.Report.Core.Service.Sales.Banner;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Mobile.V1.Sales.Controllers
{
    /// <summary>
    /// 横幅配置
    /// </summary>
    public class SalesBannerController : BaseAreaController
    {
        private readonly ISalesBannerService _salesBannerService;

        public SalesBannerController(ISalesBannerService salesBannerService)
        {
            _salesBannerService = salesBannerService;
        }

        #region 查询
        /// <summary>
        /// 获取轮播图
        /// </summary>
        /// <param name="bannerType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetBanner(BannerType bannerType)
        {
            return await _salesBannerService.GetBannerAsync(bannerType);
        }
        #endregion
    }
}
