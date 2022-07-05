using AutoMapper;
using LY.Report.Core.Model.Pay;
using LY.Report.Core.Service.Pay.Refund.Input;

namespace LY.Report.Core.Service.Pay.Refund
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<PayRefundAddInput, PayRefund>();
            CreateMap<PayRefundUpdateInput, PayRefund>();
        }
    }
}
