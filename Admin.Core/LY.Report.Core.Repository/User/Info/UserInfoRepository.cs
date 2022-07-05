using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.User;

namespace LY.Report.Core.Repository.User
{
    public class UserInfoRepository : RepositoryBase<UserInfo>, IUserInfoRepository
    {
        public UserInfoRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {

        }
    }
}
