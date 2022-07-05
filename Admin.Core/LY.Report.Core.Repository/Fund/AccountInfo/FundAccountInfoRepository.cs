using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Fund;

namespace LY.Report.Core.Repository.Fund
{

    public class FundAccountInfoRepository : RepositoryBase<FundAccountInfo>, IFundAccountInfoRepository
    {
        public FundAccountInfoRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
