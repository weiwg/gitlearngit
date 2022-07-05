using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Fund.FundRecord;
using LY.Report.Core.Service.Fund.FundRecord.Input;

namespace LY.Report.Core.Areas.Sys.V1.Fund.Controllers
{
    /// <summary>
    /// 资金管理
    /// </summary>
    public class FundController : BaseAreaController
    {
        private readonly IFundBalanceRecordService _fundBalanceRecordService;

        public FundController(IFundBalanceRecordService fundBalanceRecordService)
        {
            _fundBalanceRecordService = fundBalanceRecordService;
        }

        #region 余额
        /// <summary>
        /// 查询余额分页(后台)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetBalancePage([FromQuery] PageInput<FundBalanceRecordGetInput> model)
        {
            return await _fundBalanceRecordService.GetBalancePageAsync(model);
        }
        /// <summary>
        /// 查询冻结余额分页(后台)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetFrozenBalancePage([FromQuery] PageInput<FundBalanceRecordGetInput> model)
        {
            return await _fundBalanceRecordService.GetFrozenBalancePageAsync(model);
        }

        #endregion

        #region 充值
        /// <summary>
        /// 查询充值分页(后台)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetRechargePage([FromQuery] PageInput<FundRechargeRecordGetInput> model)
        {
            return await _fundBalanceRecordService.GetRechargePageAsync(model);
        }
        #endregion

        #region 提现
        /// <summary>
        /// 查询提现分页(后台)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetWithdrawPage([FromQuery] PageInput<FundWithdrawRecordGetInput> model)
        {
            return await _fundBalanceRecordService.GetWithdrawPageAsync(model);
        }
        #endregion

    }
}
