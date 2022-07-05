using AutoMapper;
using LY.Report.Core.Model.Mq;
using LY.Mq.Message;

namespace LY.Report.Core.Business.Mq
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<MqMessage, MqSendRecord>();
            CreateMap<MqMessage, MqReceiveRecord>();
        }
    }
}
