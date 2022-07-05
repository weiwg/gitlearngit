using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.System.Region;
using EonUp.Delivery.Core.Service.System.Region.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.System.Controllers
{
    /// <summary>
    /// 地区配置
    /// </summary>
    public class SysRegionController : BaseAreaController
    {
        private readonly ISysRegionService _sysRegionService;

        public SysRegionController(ISysRegionService sysRegionService)
        {
            _sysRegionService = sysRegionService;
        }

        #region 查询
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(SysRegionGetInput input)
        {
            return await _sysRegionService.GetOneAsync(input);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<SysRegionGetInput> model)
        {
            return await _sysRegionService.GetPageListAsync(model);
        }

        /// <summary>
        /// 获取地区详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetRegionDetailOne(SysRegionGetDetailInput input)
        {
            return await _sysRegionService.GetRegionDetailOneAsync(input);
        }

        /// <summary>
        /// 获取下拉菜单数据
        /// </summary>
        /// <param name="parentId">父级Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetSelectList(int parentId)
        {
            return await _sysRegionService.GetSelectListAsync(parentId);
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(SysRegionAddInput input)
        {
            return await _sysRegionService.AddAsync(input);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(SysRegionUpdateInput input)
        {
            return await _sysRegionService.UpdateAsync(input);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="regionId">地区id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(string regionId)
        {
            return await _sysRegionService.SoftDeleteAsync(regionId);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="regionIds"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(string[] regionIds)
        {
            return await _sysRegionService.BatchSoftDeleteAsync(regionIds);
        }
        #endregion
    }
}
