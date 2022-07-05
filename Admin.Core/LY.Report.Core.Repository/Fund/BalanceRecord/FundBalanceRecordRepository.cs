using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Fund;

namespace LY.Report.Core.Repository.Fund
{

    public class FundBalanceRecordRepository : RepositoryBase<FundBalanceRecord>, IFundBalanceRecordRepository
    {
        public FundBalanceRecordRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
