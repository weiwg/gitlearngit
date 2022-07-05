
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Auth.View.Input;
using LY.Report.Core.Service.Base.IService;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Auth.View
{
    /// <summary>
    /// ��ͼ����
    /// </summary>
    public interface IViewService:IGetService,IGetPageListService<ViewGetInput>,IAddService<ViewAddInput>,IUpdateService<ViewUpdateInput>,IDeleteService,ISoftDeleteService, IBatchSoftDeleteService
    {
        /// <summary>
        /// ��ȡȫ����ͼ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> ListAsync(ViewGetInput input);

        /// <summary>
        /// ͬ����ͼ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> SyncAsync(ViewSyncInput input);
    }
}
