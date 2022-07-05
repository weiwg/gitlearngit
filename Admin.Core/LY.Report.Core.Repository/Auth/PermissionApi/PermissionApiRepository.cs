using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Auth;

namespace LY.Report.Core.Repository.Auth.PermissionApi
{
    public class PermissionApiRepository: RepositoryBase<AuthPermissionApi>, IPermissionApiRepository
    {
        public PermissionApiRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
