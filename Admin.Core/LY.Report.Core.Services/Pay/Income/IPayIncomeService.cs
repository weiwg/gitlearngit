using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Pay.Income.Input;

namespace LY.Report.Core.Service.Pay.Income
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IPayIncomeService : IBaseService, IAddService<PayIncomeAddInput>, IUpdateService<PayIncomeUpdateInput>, IGetService<PayIncomeGetInput>
    {
        /// <summary>
        /// 处理支付状态 定时任务
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> CheckPayStatusTimerJob();
    }
}
