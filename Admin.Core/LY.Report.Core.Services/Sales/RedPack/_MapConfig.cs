using AutoMapper;
using LY.Report.Core.Model.Sales;
using LY.Report.Core.Service.Sales.RedPack.Input;

namespace LY.Report.Core.Service.Sales.RedPack
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<SalesRedPackAddInput, SalesRedPack>();
            CreateMap<SalesRedPackUpdateInput, SalesRedPack>();
        }
    }
}
