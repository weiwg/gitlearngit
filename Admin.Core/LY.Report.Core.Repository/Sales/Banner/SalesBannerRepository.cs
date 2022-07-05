using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Sales;

namespace LY.Report.Core.Repository.Sales
{

    public class SalesBannerRepository : RepositoryBase<SalesBanner>, ISalesBannerRepository
    {
        public SalesBannerRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
