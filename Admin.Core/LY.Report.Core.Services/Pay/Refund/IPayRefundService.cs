using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Pay.Refund.Input;

namespace LY.Report.Core.Service.Pay.Refund
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IPayRefundService : IBaseService, IAddService<PayRefundAddInput>, IUpdateService<PayRefundUpdateInput>, IGetService<PayRefundGetInput>
    {
        /// <summary>
        /// �������˿� ��ʱ����
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> TradeRefundTimerJob();

    }
}
