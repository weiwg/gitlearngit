using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Order;

namespace LY.Report.Core.Repository.Order
{

    public class OrderFreightCalcRepository : RepositoryBase<OrderFreightCalc>, IOrderFreightCalcRepository
    {
        public OrderFreightCalcRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
