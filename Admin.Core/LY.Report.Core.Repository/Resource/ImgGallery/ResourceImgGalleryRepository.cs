using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Resource;

namespace LY.Report.Core.Repository.Resource
{

    public class ResImgGalleryRepository : RepositoryBase<ResImgGallery>, IResImgGalleryRepository
    {
        public ResImgGalleryRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {
        }
    }
}
