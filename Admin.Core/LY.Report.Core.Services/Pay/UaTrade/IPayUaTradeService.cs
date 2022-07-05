
using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Pay.UaTrade.Input;

namespace LY.Report.Core.Service.Pay.UaTrade
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IPayUaTradeService : IBaseService
    {
        /// <summary>
        /// ����֧��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> PayOrderAsync(PayOrderAddInput input);

        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> RechargeAsync(RechargeAddInput input);

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> WithdrawAsync(WithdrawAddInput input);

    }
}
