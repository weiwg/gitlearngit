using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Auth;

namespace LY.Report.Core.Repository.Auth.Permission
{
    public class PermissionRepository : RepositoryBase<AuthPermission>, IPermissionRepository
    {
        public PermissionRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
