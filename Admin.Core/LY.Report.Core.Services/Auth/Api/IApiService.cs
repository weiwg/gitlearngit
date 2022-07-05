using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Auth.Api.Input;
using LY.Report.Core.Service.Base.IService;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Auth.Api
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IApiService:IBaseService, IAddService<ApiAddInput>, IUpdateService<ApiUpdateInput>, IGetService<ApiGetInput>, ISoftDeleteFullService<ApiDeleteInput>,IDeleteService
    {

        /// <summary>
        /// ��ȡȫ���ӿ�
        /// </summary>
        /// <param name="key"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        Task<IResponseOutput> ListAsync(string key, string apiVersion);

        /// <summary>
        /// ͬ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> SyncAsync(ApiSyncInput input);
    }
}
