
using System.Threading.Tasks;
using LY.Report.Core.Business.UaPay.Input;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.LYApiUtil.Pay.In;

namespace LY.Report.Core.Business.UaPay
{
    public interface IUaPayBusiness
    {

        #region 用户资金
        /// <summary>
        /// 用户资金(余额,冻结余额)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetUserFundAsync(string userId);
        #endregion

        #region 下单
        /// <summary>
        /// 下单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> TradeAsync(UaPayTradeIn input);

        /// <summary>
        /// 交易查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> TradeQueryAsync(UaPayTradeQueryIn input);

        /// <summary>
        /// 交易查询,分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> TradeQueryPageAsync(PageInput<UaPayTradeQueryIn> input);

        /// <summary>
        /// 担保交易解冻并打款
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> SecuredTradeUnfreezeAsync(UaPayTradeUnfreezeIn input);
        #endregion

        #region 退款
        /// <summary>
        /// 交易退款申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> TradeRefundAsync(UaPayTradeRefundIn input);

        /// <summary>
        /// 退款查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> TradeRefundQueryAsync(UaPayTradeRefundQueryIn input);

        /// <summary>
        /// 退款查询,分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> TradeRefundQueryPageAsync(PageInput<UaPayTradeRefundQueryIn> input);
        #endregion

        #region 转账

        /// <summary>
        /// 资金转账申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> TransferAsync(UaPayTransferIn input);

        /// <summary>
        /// 转账查询 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> TransferQueryAsync(UaPayTransferQueryIn input);

        /// <summary>
        /// 转账查询,分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> TradeQueryPageAsync(PageInput<UaPayTransferQueryIn> input);

        #endregion

        #region 异步通知

        /// <summary>
        /// 修改支付状态
        /// </summary>
        /// <param name="msgTradeResultIn"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateTradeStatusAsync(MsgTradeResultIn msgTradeResultIn);

        /// <summary>
        /// 修改退款状态
        /// </summary>
        /// <param name="msgRefundResultIn"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateRefundStatusAsync(MsgRefundResultIn msgRefundResultIn);

        /// <summary>
        /// 修改转账状态
        /// </summary>
        /// <param name="msgTransferResultIn"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateTransferStatusAsync(MsgTransferResultIn msgTransferResultIn);

        #endregion


        #region 查询支付状态
        /// <summary>
        /// 查询支付状态
        /// </summary>
        /// <param name="outTradeNo"></param>
        /// <returns></returns>
        Task<IResponseOutput> CheckPayStatusAsync(string outTradeNo);
        #endregion

    }
}
