using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Driver;

namespace LY.Report.Core.Repository.Driver
{
    public class DriverInfoRepository : RepositoryBase<DriverInfo>, IDriverInfoRepository
    {
        public DriverInfoRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
