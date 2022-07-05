using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Auth;

namespace LY.Report.Core.Repository.Auth.Api
{

    public class ApiRepository : RepositoryBase<AuthApi>, IApiRepository
    {
        public ApiRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
