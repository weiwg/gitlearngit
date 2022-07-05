
using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Pay.UaTrade.Input;

namespace LY.Report.Core.Service.Pay.UaTrade
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IPayUaTradeService : IBaseService
    {
        /// <summary>
        /// 订单支付
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> PayOrderAsync(PayOrderAddInput input);

        /// <summary>
        /// 余额充值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> RechargeAsync(RechargeAddInput input);

        /// <summary>
        /// 余额提现
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> WithdrawAsync(WithdrawAddInput input);

    }
}
