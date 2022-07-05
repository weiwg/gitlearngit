using AutoMapper;
using LY.Report.Core.Business.DeliveryPrice.Input;
using LY.Report.Core.Business.DeliveryPrice.Output;
using LY.Report.Core.Business.UaPay.Input;
using LY.Report.Core.Model.Order;
using LY.Report.Core.Model.Pay;
using LY.Report.Core.Service.Order.Info.Input;
using LY.Report.Core.Service.Order.Info.Output;

namespace LY.Report.Core.Service.Order.Info
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<OrderInfoAddInput, OrderInfo>();
            CreateMap<OrderInfoAddFullInput, OrderInfo>();
            CreateMap<OrderInfoGetInput, OrderInfo>();
            CreateMap<OrderInfoUpdateInput, OrderInfo>();
            CreateMap<OrderInfoDeleteInput, OrderInfo>();
            CreateMap<OrderInfo, OrderInfoGetOutput>();
            CreateMap<OrderInfo, OrderInfoGetWaitingOutput>();
            CreateMap<OrderInfo, OrderInfoFullGetOutput>();
            CreateMap<OrderInfoAddInput, OrderInfoAddFullInput>();
            CreateMap<OrderDeliveryAddInput, OrderDelivery>();
            CreateMap<DeliveryPriceGetPriceIn, OrderFreightCalc>();
            CreateMap<DeliveryPriceGetPriceOut, OrderFreightCalc>();

            CreateMap<PayIncome, UaPayTradeIn>();
            CreateMap<PayRefund, UaPayTradeRefundIn>();
             
        }
    }
}
