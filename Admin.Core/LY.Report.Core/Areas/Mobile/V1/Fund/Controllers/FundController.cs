using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Fund.FundRecord;
using LY.Report.Core.Service.Fund.FundRecord.Input;
using LY.Report.Core.Service.Pay.UaTrade;
using LY.Report.Core.Service.Pay.UaTrade.Input;

namespace LY.Report.Core.Areas.Mobile.V1.Fund.Controllers
{
    /// <summary>
    /// 资金管理
    /// </summary>
    public class FundController : BaseAreaController
    {
        private readonly IFundBalanceRecordService _fundBalanceRecordService;
        private readonly IPayUaTradeService _payUaTradeService;

        public FundController(IFundBalanceRecordService fundBalanceRecordService, IPayUaTradeService payUaTradeService)
        {
            _fundBalanceRecordService = fundBalanceRecordService;
            _payUaTradeService = payUaTradeService;
        }

        #region 余额
        /// <summary>
        /// 查询余额分页(用户)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetUserBalancePage([FromQuery] PageInput<FundBalanceRecordGetInput> model)
        {
            return await _fundBalanceRecordService.GetUserBalancePageAsync(model);
        }

        #endregion

        #region 充值
        /// <summary>
        /// 充值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Recharge(RechargeAddInput input)
        {
            return await _payUaTradeService.RechargeAsync(input);
        }
        #endregion

        #region 提现
        /// <summary>
        /// 查询提现分页(用户)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetUserWithdrawPage([FromQuery] PageInput<FundWithdrawRecordGetInput> model)
        {
            return await _fundBalanceRecordService.GetUserWithdrawPageAsync(model);
        }

        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Withdraw(WithdrawAddInput input)
        {
            return await _payUaTradeService.WithdrawAsync(input);
        }
        #endregion

    }
}
