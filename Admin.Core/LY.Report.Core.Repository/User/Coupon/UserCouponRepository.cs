using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.User;

namespace LY.Report.Core.Repository.User.RedPack
{
    public class UserCouponRepository : RepositoryBase<UserCoupon>, IUserCouponRepository
    {
        public UserCouponRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
