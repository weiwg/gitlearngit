using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.User;

namespace LY.Report.Core.Repository.User
{
    public class UserWeiXinInfoRepository : RepositoryBase<UserWeiXinInfo>, IUserWeiXinInfoRepository
    {
        public UserWeiXinInfoRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
