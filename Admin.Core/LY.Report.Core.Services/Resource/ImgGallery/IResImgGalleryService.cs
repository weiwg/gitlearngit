using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Resource.ImgGallery.Input;

namespace LY.Report.Core.Service.Resource.ImgGallery
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IResImgGalleryService : IBaseService, IAddService<ResImgGalleryAddInput>, IGetService<ResImgGalleryGetInput>, ISoftDeleteFullService<ResImgGalleryDeleteInput>
    {
    }
}
