using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Auth;

namespace LY.Report.Core.Repository.Auth.UserRole
{
    public class UserRoleRepository : RepositoryBase<AuthUserRole>, IUserRoleRepository
    {
        public UserRoleRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
