using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.System;

namespace LY.Report.Core.Repository.Admin
{
    public  class TenantRepository : RepositoryBase<SysTenant>, ITenantRepository
    {
        public TenantRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
