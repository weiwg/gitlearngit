using AutoMapper;
using LY.Report.Core.Model.Delivery;
using LY.Report.Core.Service.Delivery.CarType.Input;

namespace LY.Report.Core.Service.Delivery.CarType
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<DeliveryCarTypeAddInput, DeliveryCarType>();
            CreateMap<DeliveryCarTypeUpdateInput, DeliveryCarType>();
        }
    }
}
