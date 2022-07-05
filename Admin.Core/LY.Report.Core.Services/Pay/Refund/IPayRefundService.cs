using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Pay.Refund.Input;

namespace LY.Report.Core.Service.Pay.Refund
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IPayRefundService : IBaseService, IAddService<PayRefundAddInput>, IUpdateService<PayRefundUpdateInput>, IGetService<PayRefundGetInput>
    {
        /// <summary>
        /// 处理订单退款 定时任务
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> TradeRefundTimerJob();

    }
}
