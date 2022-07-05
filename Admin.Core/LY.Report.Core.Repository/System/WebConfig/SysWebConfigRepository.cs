using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.System;

namespace LY.Report.Core.Repository.System
{

    public class SysWebConfigRepository : RepositoryBase<SysWebConfig>, ISysWebConfigRepository
    {
        public SysWebConfigRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
