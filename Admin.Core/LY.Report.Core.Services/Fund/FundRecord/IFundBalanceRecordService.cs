using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Fund.FundRecord.Input;

namespace LY.Report.Core.Service.Fund.FundRecord
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IFundBalanceRecordService : IBaseService
    {
        /// <summary>
        /// 余额记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetUserBalancePageAsync(PageInput<FundBalanceRecordGetInput> input);

        /// <summary>
        /// 余额记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetBalancePageAsync(PageInput<FundBalanceRecordGetInput> input);

        /// <summary>
        /// 冻结余额记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetUserFrozenBalancePageAsync(PageInput<FundBalanceRecordGetInput> input);

        /// <summary>
        /// 冻结余额记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetFrozenBalancePageAsync(PageInput<FundBalanceRecordGetInput> input);

        /// <summary>
        /// 充值记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetUserRechargePageAsync(PageInput<FundRechargeRecordGetInput> input);

        /// <summary>
        /// 充值记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetRechargePageAsync(PageInput<FundRechargeRecordGetInput> input);

        /// <summary>
        /// 提现记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetUserWithdrawPageAsync(PageInput<FundWithdrawRecordGetInput> input);

        /// <summary>
        /// 提现记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetWithdrawPageAsync(PageInput<FundWithdrawRecordGetInput> input);
    }
}
