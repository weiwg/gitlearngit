using AutoMapper;
using LY.Report.Core.LYApiUtil.Pay.In;
using LY.Report.Core.LYApiUtil.Pay.Out;
using LY.Report.Core.Model.Pay;

namespace LY.Report.Core.Business.UaPay
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<MsgTradeResultIn, PayIncome>();
            CreateMap<MsgRefundResultIn, PayRefund>();
            CreateMap<MsgTransferResultIn, PayTransfer>();

            CreateMap<TradeQueryGetOutput, MsgTradeResultIn>();
        }
    }
}
