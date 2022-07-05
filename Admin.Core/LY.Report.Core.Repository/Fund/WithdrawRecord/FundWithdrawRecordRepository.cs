using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Fund;

namespace LY.Report.Core.Repository.Fund
{

    public class FundWithdrawRecordRepository : RepositoryBase<FundWithdrawRecord>, IFundWithdrawRecordRepository
    {
        public FundWithdrawRecordRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
