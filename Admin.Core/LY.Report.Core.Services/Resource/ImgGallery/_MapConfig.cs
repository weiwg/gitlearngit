using AutoMapper;
using LY.Report.Core.Model.Resource;
using LY.Report.Core.Service.Resource.ImgGallery.Input;

namespace LY.Report.Core.Service.Resource.ImgGallery
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<ResImgGalleryAddInput, ResImgGallery>();
        }
    }
}
