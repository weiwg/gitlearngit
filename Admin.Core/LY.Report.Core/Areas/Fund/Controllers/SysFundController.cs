using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.Fund.SysFundRecord;
using EonUp.Delivery.Core.Service.Fund.SysFundRecord.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Fund.Controllers
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
        [HttpPost]
        public async Task<IResponseOutput> GetAppBalancePage(PageInput<SysFundBalanceRecordGetInput> input)
        {
            return await _fundSysFundsService.GetAppBalancePageAsync(input);
        }

        /// <summary>
        /// 查询冻结余额分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetAppFrozenBalancePage(PageInput<SysFundBalanceRecordGetInput> input)
        {
            return await _fundSysFundsService.GetAppFrozenBalancePageAsync(input);
        }
    }
}
