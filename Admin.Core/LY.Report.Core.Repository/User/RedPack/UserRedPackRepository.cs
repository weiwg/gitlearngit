using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.User;

namespace LY.Report.Core.Repository.User.RedPack
{
    public  class UserRedPackRepository : RepositoryBase<UserRedPack>, IUserRedPackRepository
    {
        public UserRedPackRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
