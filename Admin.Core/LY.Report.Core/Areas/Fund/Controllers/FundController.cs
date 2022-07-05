using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.Fund.FundRecord;
using EonUp.Delivery.Core.Service.Fund.FundRecord.Input;
using EonUp.Delivery.Core.Service.Pay.UaTrade;
using EonUp.Delivery.Core.Service.Pay.UaTrade.Input;

namespace EonUp.Delivery.Core.Areas.Fund.Controllers
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
        [HttpPost]
        public async Task<IResponseOutput> GetUserBalancePage(PageInput<FundBalanceRecordGetInput> model)
        {
            return await _fundBalanceRecordService.GetUserBalancePageAsync(model);
        }

        /// <summary>
        /// 查询余额分页(后台)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetBalancePage(PageInput<FundBalanceRecordGetInput> model)
        {
            return await _fundBalanceRecordService.GetBalancePageAsync(model);
        }

        /// <summary>
        /// 查询冻结余额分页(用户)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetUserFrozenBalancePage(PageInput<FundBalanceRecordGetInput> model)
        {
            return await _fundBalanceRecordService.GetUserFrozenBalancePageAsync(model);
        }

        /// <summary>
        /// 查询冻结余额分页(后台)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetFrozenBalancePage(PageInput<FundBalanceRecordGetInput> model)
        {
            return await _fundBalanceRecordService.GetFrozenBalancePageAsync(model);
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

        /// <summary>
        /// 查询充值分页(用户)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetUserRechargePage(PageInput<FundRechargeRecordGetInput> model)
        {
            return await _fundBalanceRecordService.GetUserRechargePageAsync(model);
        }

        /// <summary>
        /// 查询充值分页(后台)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetRechargePage(PageInput<FundRechargeRecordGetInput> model)
        {
            return await _fundBalanceRecordService.GetRechargePageAsync(model);
        }
        #endregion

        #region 提现

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

        /// <summary>
        /// 查询提现分页(用户)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetUserWithdrawPage(PageInput<FundWithdrawRecordGetInput> model)
        {
            return await _fundBalanceRecordService.GetUserWithdrawPageAsync(model);
        }

        /// <summary>
        /// 查询提现分页(后台)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetWithdrawPage(PageInput<FundWithdrawRecordGetInput> model)
        {
            return await _fundBalanceRecordService.GetWithdrawPageAsync(model);
        }
        #endregion

    }
}
