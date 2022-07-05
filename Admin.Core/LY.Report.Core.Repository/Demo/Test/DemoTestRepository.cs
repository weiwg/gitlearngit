using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Demo;

namespace LY.Report.Core.Repository.Demo
{

    public class DemoTestRepository : RepositoryBase<DemoTest>, IDemoTestRepository
    {
        public DemoTestRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
