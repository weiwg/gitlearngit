using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Fund.SysFundRecord.Input;

namespace LY.Report.Core.Service.Fund.SysFundRecord
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface ISysFundBalanceRecordService : IBaseService
    {
        /// <summary>
        /// 系统资金(余额,冻结余额)
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetAppFundAsync();

        /// <summary>
        /// 余额记录
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetAppBalancePageAsync(PageInput<SysFundBalanceRecordGetInput> input);

        /// <summary>
        /// 冻结余额记录
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetAppFrozenBalancePageAsync(PageInput<SysFundBalanceRecordGetInput> input);
    }
}
