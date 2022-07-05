using AutoMapper;
using LY.Report.Core.Model.Resource;
using LY.Report.Core.Service.Resource.ImgData.Input;

namespace LY.Report.Core.Service.Resource.ImgData
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<ResImgDataAddInput, ResImgData>();
            CreateMap<ResImgDataUpdateInput, ResImgData>();
        }
    }
}
