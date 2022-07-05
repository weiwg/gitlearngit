using AutoMapper;
using LY.Report.Core.Model.Mq;
using LY.Report.Core.Service.Mq.ReceiveRecord.Input;

namespace LY.Report.Core.Service.Mq.ReceiveRecord
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<MqReceiveRecordAddInput, MqReceiveRecord>();
            CreateMap<MqReceiveRecordUpdateInput, MqReceiveRecord>();
        }
    }
}
