using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.System.Region;
using LY.Report.Core.Service.System.Region.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Sys.V1.System.Controllers
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
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<SysRegionGetInput> model)
        {
            return await _sysRegionService.GetPageListAsync(model);
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
