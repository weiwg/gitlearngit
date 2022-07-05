using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Order;

namespace LY.Report.Core.Repository.Order
{

    public class OrderInfoRepository : RepositoryBase<OrderInfo>, IOrderInfoRepository
    {
        public OrderInfoRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
