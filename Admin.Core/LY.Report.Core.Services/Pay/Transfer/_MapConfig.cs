using AutoMapper;
using LY.Report.Core.Model.Pay;
using LY.Report.Core.Service.Pay.Transfer.Input;

namespace LY.Report.Core.Service.Pay.Transfer
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<PayTransferAddInput, PayTransfer>();
            CreateMap<PayTransferUpdateInput, PayTransfer>();
        }
    }
}
