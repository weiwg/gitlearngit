using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.System.ParamConfig;
using LY.Report.Core.Service.System.ParamConfig.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Other.V1.System.Controllers
{
    /// <summary>
    /// 系统参数配置
    /// </summary>
    public class SysParamConfigController : BaseAreaController
    {
        private readonly ISysParamConfigService _sysParamConfigService;

        public SysParamConfigController(ISysParamConfigService paramConfigService)
        {
            _sysParamConfigService = paramConfigService;
        }

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string id)
        {
            return await _sysParamConfigService.GetOneAsync(id);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<SysParamConfigGetInput> model)
        {
            return await _sysParamConfigService.GetPageListAsync(model);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(SysParamConfigAddInput input)
        {
            return await _sysParamConfigService.AddAsync(input);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(SysParamConfigUpdateInput input)
        {
            return await _sysParamConfigService.UpdateAsync(input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(string id)
        {
            return await _sysParamConfigService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        {
            return await _sysParamConfigService.BatchSoftDeleteAsync(ids);
        }
    }
}
