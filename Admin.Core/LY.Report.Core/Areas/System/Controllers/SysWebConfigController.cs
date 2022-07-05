using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.System.WebConfig;
using EonUp.Delivery.Core.Service.System.WebConfig.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.System.Controllers
{
    /// <summary>
    /// 网站配置
    /// </summary>
    public class SysWebConfigController : BaseAreaController
    {
        private readonly ISysWebConfigService _sysWebConfigService;

        public SysWebConfigController(ISysWebConfigService sysWebConfigService)
        {
            _sysWebConfigService = sysWebConfigService;
        }

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string id)
        {
            return await _sysWebConfigService.GetOneAsync(id);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<SysWebConfigGetInput> model)
        {
            return await _sysWebConfigService.GetPageListAsync(model);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(SysWebConfigAddInput input)
        {
            return await _sysWebConfigService.AddAsync(input);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(SysWebConfigUpdateInput input)
        {
            return await _sysWebConfigService.UpdateAsync(input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(string id)
        {
            return await _sysWebConfigService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        {
            return await _sysWebConfigService.BatchSoftDeleteAsync(ids);
        }
    }
}
