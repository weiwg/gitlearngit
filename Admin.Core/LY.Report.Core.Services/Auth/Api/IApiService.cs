using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Auth.Api.Input;
using LY.Report.Core.Service.Base.IService;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Auth.Api
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IApiService:IBaseService, IAddService<ApiAddInput>, IUpdateService<ApiUpdateInput>, IGetService<ApiGetInput>, ISoftDeleteFullService<ApiDeleteInput>,IDeleteService
    {

        /// <summary>
        /// 获取全部接口
        /// </summary>
        /// <param name="key"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        Task<IResponseOutput> ListAsync(string key, string apiVersion);

        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> SyncAsync(ApiSyncInput input);
    }
}
