using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Driver;

namespace LY.Report.Core.Repository.Driver
{
    public class DriverIdentityInfoRepository : RepositoryBase<DriverIdentityInfo>, IDriverIdentityInfoRepository
    {
        public DriverIdentityInfoRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
