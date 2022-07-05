using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Order.Evaluation.Input;

namespace LY.Report.Core.Service.Order.Evaluation
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IOrderEvaluationService : IBaseService, IGetService<OrderEvaluationGetInput>, ISoftDeleteFullService<OrderEvaluationDeleteInput>
    {
        /// <summary>
        /// 新增用户评价(司机评价用户)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddUserAsync(OrderEvaluationAddInput input);

        /// <summary>
        /// 新增司机评价(用户评价司机)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddDriverAsync(OrderEvaluationAddInput input);
    }
}
