
using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Pay.Enum;

namespace LY.Report.Core.Business.Pay
{
    public interface IPayBusiness
    {
        
        /// <summary>
        /// 取消交易订单(包括外部订单)
        /// </summary>
        /// <param name="outTradeNo"></param>
        /// <param name="payOrderType"></param>
        /// <returns></returns>
        Task<IResponseOutput> CloseOutTradeAsync(string outTradeNo, PayOrderType payOrderType);

        /// <summary>
        /// 取消交易订单
        /// </summary>
        /// <param name="outTradeNo"></param>
        /// <param name="payOrderType"></param>
        /// <returns></returns>
        Task<IResponseOutput> CloseTradeAsync(string outTradeNo, PayOrderType payOrderType);


    }
}
