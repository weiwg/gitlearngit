using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Auth;

namespace LY.Report.Core.Repository.Auth.RolePermisson
{
    public class RolePermissonRepository: RepositoryBase<AuthRolePermission>,IRolePermissonRepository
    {
        public RolePermissonRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
