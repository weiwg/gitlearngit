using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.LYApiUtil.Pay;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Fund.FundRecord.Input;

namespace LY.Report.Core.Service.Fund.FundRecord
{
    public class FundBalanceRecordService : BaseService, IFundBalanceRecordService
    {

        public FundBalanceRecordService()
        {
        }

        #region Óà¶î¼ÇÂ¼

        public async Task<IResponseOutput> GetUserBalancePageAsync(PageInput<FundBalanceRecordGetInput> input)
        {
            input.Filter = input.Filter ?? new FundBalanceRecordGetInput();
            input.Filter.UserId = User.UserId;
            if (input.Filter.UserId.IsNull())
            {
                return ResponseOutput.NotOk("Î´µÇÂ¼");
            }

            return await GetBalancePageAsync(input);
        }

        public async Task<IResponseOutput> GetBalancePageAsync(PageInput<FundBalanceRecordGetInput> input)
        {
            input.Filter = input.Filter ?? new FundBalanceRecordGetInput();
            if (input.Filter.UserId.IsNull())
            {
                return ResponseOutput.NotOk("ÓÃ»§Id´íÎó");
            }

            var apiResult = await PayApiHelper.GetBalancePageAsync(input.Filter, input.CurrentPage, input.PageSize);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Data(apiResult.Data);
        }

        public async Task<IResponseOutput> GetUserFrozenBalancePageAsync(PageInput<FundBalanceRecordGetInput> input)
        {
            input.Filter = input.Filter ?? new FundBalanceRecordGetInput();
            input.Filter.UserId = User.UserId;
            if (input.Filter.UserId.IsNull())
            {
                return ResponseOutput.NotOk("Î´µÇÂ¼");
            }
            return await GetFrozenBalancePageAsync(input);
        }

        public async Task<IResponseOutput> GetFrozenBalancePageAsync(PageInput<FundBalanceRecordGetInput> input)
        {
            input.Filter = input.Filter ?? new FundBalanceRecordGetInput();
            if (input.Filter.UserId.IsNull())
            {
                return ResponseOutput.NotOk("ÓÃ»§Id´íÎó");
            }

            var apiResult = await PayApiHelper.GetFrozenBalancePageAsync(input.Filter, input.CurrentPage, input.PageSize);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Data(apiResult.Data);
        }
        #endregion

        #region ³äÖµ¼ÇÂ¼
        public async Task<IResponseOutput> GetUserRechargePageAsync(PageInput<FundRechargeRecordGetInput> input)
        {
            input.Filter = input.Filter ?? new FundRechargeRecordGetInput();
            input.Filter.UserId = User.UserId;
            if (input.Filter.UserId.IsNull())
            {
                return ResponseOutput.NotOk("Î´µÇÂ¼");
            }
            return await GetRechargePageAsync(input);
        }

        public async Task<IResponseOutput> GetRechargePageAsync(PageInput<FundRechargeRecordGetInput> input)
        {
            input.Filter = input.Filter ?? new FundRechargeRecordGetInput();
            if (input.Filter.UserId.IsNull())
            {
                return ResponseOutput.NotOk("ÓÃ»§Id´íÎó");
            }

            var apiResult = await PayApiHelper.GetRechargePageAsync(input.Filter, input.CurrentPage, input.PageSize);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Data(apiResult.Data);
        }
        #endregion

        #region ÌáÏÖ¼ÇÂ¼
        public async Task<IResponseOutput> GetUserWithdrawPageAsync(PageInput<FundWithdrawRecordGetInput> input)
        {
            input.Filter = input.Filter ?? new FundWithdrawRecordGetInput();
            input.Filter.UserId = User.UserId;
            if (input.Filter.UserId.IsNull())
            {
                return ResponseOutput.NotOk("Î´µÇÂ¼");
            }
            return await GetWithdrawPageAsync(input);
        }

        public async Task<IResponseOutput> GetWithdrawPageAsync(PageInput<FundWithdrawRecordGetInput> input)
        {
            input.Filter = input.Filter ?? new FundWithdrawRecordGetInput();
            if (input.Filter.UserId.IsNull())
            {
                return ResponseOutput.NotOk("ÓÃ»§Id´íÎó");
            }

            var apiResult = await PayApiHelper.GetWithdrawPageAsync(input.Filter, input.CurrentPage, input.PageSize);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Data(apiResult.Data);
        }
        #endregion
    }
}
