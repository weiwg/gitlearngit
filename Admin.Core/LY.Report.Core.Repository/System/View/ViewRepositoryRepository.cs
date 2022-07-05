using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Auth;

namespace LY.Report.Core.Repository.Admin
{
    public class ViewRepository : RepositoryBase<AuthView>, IViewRepository
    {
        public ViewRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {

        }
    }
}
