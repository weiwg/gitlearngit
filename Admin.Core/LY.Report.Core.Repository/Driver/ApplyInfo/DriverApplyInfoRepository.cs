using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Driver;

namespace LY.Report.Core.Repository.Driver
{

    public class DriverApplyInfoRepository : RepositoryBase<DriverApplyInfo>, IDriverApplyInfoRepository
    {
        public DriverApplyInfoRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
