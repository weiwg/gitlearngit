using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Sales.RedPack.Input;

namespace LY.Report.Core.Service.Sales.RedPack
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface ISalesRedPackService : IBaseService, IAddService<SalesRedPackAddInput>, IUpdateEntityService<SalesRedPackUpdateInput>, IGetService<SalesRedPackGetInput>, ISoftDeleteFullService<SalesRedPackDeleteInput>
    {
        /// <summary>
        /// 检查红包状态 定时任务
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> CheckSysRedPackStatusTimerJob();
    }
}
