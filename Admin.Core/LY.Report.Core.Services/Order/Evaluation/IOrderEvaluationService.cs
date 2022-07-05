using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Order.Evaluation.Input;

namespace LY.Report.Core.Service.Order.Evaluation
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IOrderEvaluationService : IBaseService, IGetService<OrderEvaluationGetInput>, ISoftDeleteFullService<OrderEvaluationDeleteInput>
    {
        /// <summary>
        /// �����û�����(˾�������û�)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddUserAsync(OrderEvaluationAddInput input);

        /// <summary>
        /// ����˾������(�û�����˾��)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddDriverAsync(OrderEvaluationAddInput input);
    }
}
