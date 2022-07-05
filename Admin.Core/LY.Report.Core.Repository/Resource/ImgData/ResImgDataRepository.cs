using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Resource;

namespace LY.Report.Core.Repository.Resource
{

    public class ResImgDataRepository : RepositoryBase<ResImgData>, IResImgDataRepository
    {
        public ResImgDataRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
