using AutoMapper;
using LY.Report.Core.Model.Sales;
using LY.Report.Core.Service.Sales.Banner.Input;
using LY.Report.Core.Service.Sales.Banner.Output;

namespace LY.Report.Core.Service.Sales.Banner
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<SalesBannerAddInput, SalesBanner>();
            CreateMap<SalesBannerUpdateInput, SalesBanner>();

            CreateMap<SalesBanner,SalesBannerListOutput> ();
        }
    }
}
