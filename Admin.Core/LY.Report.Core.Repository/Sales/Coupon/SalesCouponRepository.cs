using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Sales;

namespace LY.Report.Core.Repository.Sales.Coupon
{
   public class SalesCouponRepository : RepositoryBase<SalesCoupon>, ISalesCouponRepository
    {
        public SalesCouponRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
