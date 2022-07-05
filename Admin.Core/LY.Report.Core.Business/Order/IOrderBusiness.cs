
using System.Threading.Tasks;
using LY.Report.Core.Common.Output;

namespace LY.Report.Core.Business.Order
{
    public interface IOrderBusiness
    {
        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="outTradeNo"></param>
        /// <param name="cancelReason"></param>
        /// <returns></returns>
        Task<IResponseOutput> OrderRefundAsync(string outTradeNo, string cancelReason);

        /// <summary>
        /// 退回抵扣(优惠券,红包,积分)
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        Task<IResponseOutput> OrderRefundDeductionAsync(string orderNo);
    }
}
