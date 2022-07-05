using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Order;

namespace LY.Report.Core.Repository.Order
{

    public class OrderDeliveryRepository : RepositoryBase<OrderDelivery>, IOrderDeliveryRepository
    {
        public OrderDeliveryRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
