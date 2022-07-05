using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Auth.Api;
using LY.Report.Core.Service.Auth.Api.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Sys.V1.Auth.Controllers
{
    /// <summary>
    /// 接口管理
    /// </summary>
    public class ApiController : BaseAreaController
    {
        private readonly IApiService _apiService;

        public ApiController(IApiService apiService)
        {
            _apiService = apiService;
        }

        #region 新增

        /// <summary>
        /// 新增接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(ApiAddInput input)
        {
            return await _apiService.AddAsync(input);
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(string id)
        {
            return await _apiService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除接口
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        {
            return await _apiService.BatchSoftDeleteAsync(ids);
        }
        #endregion

        #region 查询
        /// <summary>
        /// 查询单条接口
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string id)
        {
            return await _apiService.GetOneAsync(id);
        }

        /// <summary>
        /// 查询全部接口
        /// </summary>
        /// <param name="key"></param>
        /// <param name="apiVersion">版本号</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetList(string key, string apiVersion)
        {
            return await _apiService.ListAsync(key, apiVersion);
        }

        /// <summary>
        /// 查询分页接口
        /// </summary>
        /// <param name="model">分页模型</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<ApiGetInput> model)
        {
            return await _apiService.GetPageListAsync(model);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(ApiUpdateInput input)
        {
            return await _apiService.UpdateAsync(input);
        }
        #endregion

        #region 同步api
        /// <summary>
        /// 同步接口
        /// 支持新增和修改接口
        /// 根据接口是否存在自动禁用和启用api
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Sync(ApiSyncInput input)
        {
            return await _apiService.SyncAsync(input);
        }
        #endregion
    }
}
