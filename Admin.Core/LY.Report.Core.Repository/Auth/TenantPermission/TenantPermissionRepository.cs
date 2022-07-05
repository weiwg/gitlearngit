using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Auth;

namespace LY.Report.Core.Repository.Auth.TenantPermission
{
    public class TenantPermissionRepository: RepositoryBase<AuthTenantPermission>, ITenantPermissionRepository
    {
        public TenantPermissionRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
