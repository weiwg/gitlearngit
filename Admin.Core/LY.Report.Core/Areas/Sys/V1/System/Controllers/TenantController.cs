using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.System.Tenant;
using LY.Report.Core.Service.System.Tenant.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Sys.V1.System.Controllers
{
    /// <summary>
    /// 租户管理
    /// </summary>
    public class TenantController : BaseAreaController
    {
        private readonly ITenantService _tenantServices;

        public TenantController(ITenantService tenantService)
        {
            _tenantServices = tenantService;
        }

        #region 新增
        /// <summary>
        /// 新增租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(TenantAddInput input)
        {
            return await _tenantServices.AddAsync(input);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 彻底删除租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> Delete(string id)
        {
            return await _tenantServices.DeleteAsync(id);
        }


        /// <summary>
        /// 删除租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(string id)
        {
            return await _tenantServices.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除租户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        {
            return await _tenantServices.BatchSoftDeleteAsync(ids);
        }
        #endregion

        #region 查询
        /// <summary>
        /// 查询单条租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string id)
        {
            return await _tenantServices.GetOneAsync(id);
        }

        /// <summary>
        /// 查询分页租户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<TenantGetInput> model)
        {
            return await _tenantServices.GetPageListAsync(model);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(TenantUpdateInput input)
        {
            return await _tenantServices.UpdateAsync(input);
        }
        #endregion
    }
}
