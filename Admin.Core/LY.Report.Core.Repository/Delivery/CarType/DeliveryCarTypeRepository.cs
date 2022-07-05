using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Delivery;

namespace LY.Report.Core.Repository.Delivery
{

    public class DeliveryCarTypeRepository : RepositoryBase<DeliveryCarType>, IDeliveryCarTypeRepository
    {
        public DeliveryCarTypeRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
