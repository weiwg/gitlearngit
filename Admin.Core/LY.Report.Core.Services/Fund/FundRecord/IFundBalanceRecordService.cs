using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Fund.FundRecord.Input;

namespace LY.Report.Core.Service.Fund.FundRecord
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IFundBalanceRecordService : IBaseService
    {
        /// <summary>
        /// ����¼
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetUserBalancePageAsync(PageInput<FundBalanceRecordGetInput> input);

        /// <summary>
        /// ����¼
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetBalancePageAsync(PageInput<FundBalanceRecordGetInput> input);

        /// <summary>
        /// ��������¼
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetUserFrozenBalancePageAsync(PageInput<FundBalanceRecordGetInput> input);

        /// <summary>
        /// ��������¼
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetFrozenBalancePageAsync(PageInput<FundBalanceRecordGetInput> input);

        /// <summary>
        /// ��ֵ��¼
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetUserRechargePageAsync(PageInput<FundRechargeRecordGetInput> input);

        /// <summary>
        /// ��ֵ��¼
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetRechargePageAsync(PageInput<FundRechargeRecordGetInput> input);

        /// <summary>
        /// ���ּ�¼
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetUserWithdrawPageAsync(PageInput<FundWithdrawRecordGetInput> input);

        /// <summary>
        /// ���ּ�¼
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetWithdrawPageAsync(PageInput<FundWithdrawRecordGetInput> input);
    }
}
