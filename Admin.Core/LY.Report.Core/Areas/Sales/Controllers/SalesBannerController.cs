using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Model.Sales.Enum;
using EonUp.Delivery.Core.Service.Sales.Banner;
using EonUp.Delivery.Core.Service.Sales.Banner.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Sales.Controllers
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
        /// 查询单条
        /// </summary>
        /// <param name="bannerId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string bannerId)
        {
            return await _salesBannerService.GetOneAsync(bannerId);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<SalesBannerGetInput> model)
        {
            return await _salesBannerService.GetPageListAsync(model);
        }

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

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(SalesBannerAddInput input)
        {
            return await _salesBannerService.AddAsync(input);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(SalesBannerUpdateInput input)
        {
            return await _salesBannerService.UpdateEntityAsync(input);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="bannerId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(string bannerId)
        {
            return await _salesBannerService.SoftDeleteAsync(bannerId);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        {
            return await _salesBannerService.BatchSoftDeleteAsync(ids);
        }
        #endregion
    }
}
