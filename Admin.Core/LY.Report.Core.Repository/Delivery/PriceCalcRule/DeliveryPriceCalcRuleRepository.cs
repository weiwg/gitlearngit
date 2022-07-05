using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Delivery;

namespace LY.Report.Core.Repository.Delivery
{

    public class DeliveryPriceCalcRuleRepository : RepositoryBase<DeliveryPriceCalcRule>, IDeliveryPriceCalcRuleRepository
    {
        public DeliveryPriceCalcRuleRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
