using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Auth;

namespace LY.Report.Core.Repository.Auth.Role
{
    public  class RoleRepository : RepositoryBase<AuthRole>, IRoleRepository
    {
        public RoleRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
