using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.System;

namespace LY.Report.Core.Repository.System
{

    public class SysParamConfigRepository : RepositoryBase<SysParamConfig>, ISysParamConfigRepository
    {
        public SysParamConfigRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
