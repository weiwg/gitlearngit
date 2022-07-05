using System.Threading.Tasks;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.LYApiUtil.Pay;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Fund.AccountInfo.Input;

namespace LY.Report.Core.Service.Fund.AccountInfo
{
    public class FundAccountInfoService : BaseService, IFundAccountInfoService
    {
        public FundAccountInfoService(IUser user)
        {
        }

        public async Task<IResponseOutput> AddFundAccountAsync(FundAccountInfoAddInput input)
        {
            input.UserId = User.UserId;
            var apiResult = await PayApiHelper.AddFundAccountAsync(input);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Ok(apiResult.Msg);
        }

        public async Task<IResponseOutput> UpdateFundAccountAsync(FundAccountInfoUpdateInput input)
        {
            input.UserId = User.UserId;
            var apiResult = await PayApiHelper.UpdateFundAccountAsync(input);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Ok(apiResult.Msg);
        }

        public async Task<IResponseOutput> GetFundAccountAsync(FundAccountInfoGetInput input)
        {
            input.UserId = User.UserId;
            var apiResult = await PayApiHelper.GetFundAccountAsync(input);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Data(apiResult.Data);
        }

        public async Task<IResponseOutput> GetFundAccountPageAsync(PageInput<FundAccountInfoGetInput> input)
        {
            input.Filter = input.Filter ?? new FundAccountInfoGetInput();
            input.Filter.UserId = User.UserId;
            var apiResult = await PayApiHelper.GetFundAccountPageAsync(input.Filter, input.CurrentPage, input.PageSize);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Data(apiResult.Data);
        }

        public async Task<IResponseOutput> DeleteFundAccountAsync(FundAccountInfoDeleteInput input)
        {
            input.UserId = User.UserId;
            var apiResult = await PayApiHelper.DeleteFundAccountAsync(input);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Ok(apiResult.Msg);
        }
    }
}
