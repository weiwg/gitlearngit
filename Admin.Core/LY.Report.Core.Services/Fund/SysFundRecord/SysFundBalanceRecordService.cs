using System.Threading.Tasks;
using AutoMapper;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.LYApiUtil.Pay;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Fund.SysFundRecord.Input;

namespace LY.Report.Core.Service.Fund.SysFundRecord
{
   public class SysFundBalanceRecordService: BaseService, ISysFundBalanceRecordService
    {
        public SysFundBalanceRecordService()
        {
        }

        public async Task<IResponseOutput> GetAppFundAsync()
        {
            var apiResult = await PayApiHelper.GetAppFundAsync();
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Ok(apiResult.Msg, apiResult.Data);
        }


        #region 系统余额(冻结余额)
        public async Task<IResponseOutput> GetAppBalancePageAsync(PageInput<SysFundBalanceRecordGetInput> input)
        {
            input.Filter = input.Filter ?? new SysFundBalanceRecordGetInput();
            var apiResult = await PayApiHelper.GetAppBalancePageAsync(input.Filter,input.CurrentPage, input.PageSize);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Data(apiResult);
        }

        public async Task<IResponseOutput> GetAppFrozenBalancePageAsync(PageInput<SysFundBalanceRecordGetInput> input)
        {
            input.Filter = input.Filter ?? new SysFundBalanceRecordGetInput();
            var apiResult = await PayApiHelper.GetAppFrozenBalancePageAsync(input.Filter, input.CurrentPage, input.PageSize);
            if (!apiResult.Success)
            {
                return ResponseOutput.NotOk(apiResult.Msg);
            }
            return ResponseOutput.Data(apiResult);
        }
        #endregion
    }
}
