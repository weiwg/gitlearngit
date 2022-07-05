using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Order;

namespace LY.Report.Core.Repository.Order
{

    public class OrderDeductionRepository : RepositoryBase<OrderDeduction>, IOrderDeductionRepository
    {
        public OrderDeductionRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
