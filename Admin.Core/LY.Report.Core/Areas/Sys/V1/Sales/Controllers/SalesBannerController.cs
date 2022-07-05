using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Sales.Banner;
using LY.Report.Core.Service.Sales.Banner.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Sys.V1.Sales.Controllers
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
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<SalesBannerGetInput> model)
        {
            return await _salesBannerService.GetPageListAsync(model);
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
