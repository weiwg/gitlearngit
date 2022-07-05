using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Order;

namespace LY.Report.Core.Repository.Order
{

    public class OrderEvaluationRepository : RepositoryBase<OrderEvaluation>, IOrderEvaluationRepository
    {
        public OrderEvaluationRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
