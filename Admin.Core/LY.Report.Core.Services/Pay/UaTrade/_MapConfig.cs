using AutoMapper;
using LY.Report.Core.Business.UaPay.Input;
using LY.Report.Core.Model.Pay;
using LY.Report.Core.Service.Pay.UaTrade.Input;

namespace LY.Report.Core.Service.Pay.UaTrade
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<WithdrawAddInput, PayTransfer>();
            CreateMap<PayTransfer, UaPayTransferIn>();
        }
    }
}
