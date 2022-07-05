using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Fund.SysFundRecord;
using LY.Report.Core.Service.Fund.SysFundRecord.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Sys.V1.Fund.Controllers
{
    /// <summary>
    /// 系统资金管理
    /// </summary>
    public class SysFundController : BaseAreaController
    {
        private readonly ISysFundBalanceRecordService _fundSysFundsService;

        public SysFundController(ISysFundBalanceRecordService fundSysFundsService)
        {
            _fundSysFundsService = fundSysFundsService;
        }

        /// <summary>
        /// 系统资金(余额,冻结余额)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetBalance()
        {
            return await _fundSysFundsService.GetAppFundAsync();
        }
        /// <summary>
        /// 查询余额分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetAppBalancePage([FromQuery] PageInput<SysFundBalanceRecordGetInput> input)
        {
            return await _fundSysFundsService.GetAppBalancePageAsync(input);
        }

        /// <summary>
        /// 查询冻结余额分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetAppFrozenBalancePage([FromQuery] PageInput<SysFundBalanceRecordGetInput> input)
        {
            return await _fundSysFundsService.GetAppFrozenBalancePageAsync(input);
        }
    }
}
