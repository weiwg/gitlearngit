using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.System;

namespace LY.Report.Core.Repository.System
{

    public class SysRegionRepository : RepositoryBase<SysRegion>, ISysRegionRepository
    {
        public SysRegionRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
