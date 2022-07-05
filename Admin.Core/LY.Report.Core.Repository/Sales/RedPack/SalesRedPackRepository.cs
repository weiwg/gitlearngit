using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Sales;

namespace LY.Report.Core.Repository.Sales
{

    public class SalesRedPackRepository : RepositoryBase<SalesRedPack>, ISalesRedPackRepository
    {
        public SalesRedPackRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
