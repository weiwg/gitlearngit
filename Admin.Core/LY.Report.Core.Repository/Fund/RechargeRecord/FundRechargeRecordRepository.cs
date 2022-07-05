using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Fund;

namespace LY.Report.Core.Repository.Fund
{

    public class FundRechargeRecordRepository : RepositoryBase<FundRechargeRecord>, IFundRechargeRecordRepository
    {
        public FundRechargeRecordRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
