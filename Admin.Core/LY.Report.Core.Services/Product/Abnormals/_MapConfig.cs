using AutoMapper;
using LY.Report.Core.Model.Product;
using LY.Report.Core.Service.Product.Abnormals.Input;

namespace LY.Report.Core.Service.Product.Abnormals
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<ProductAbnormalAddInput, Abnormal>();
            CreateMap<ProductAbnormalUpdateInput, Abnormal>();
        }
    }
}
