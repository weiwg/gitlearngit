using AutoMapper;
using LY.Report.Core.Model.Mq;
using LY.Report.Core.Service.Mq.SendRecord.Input;

namespace LY.Report.Core.Service.Mq.SendRecord
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<MqSendRecordAddInput, MqSendRecord>();
            CreateMap<MqSendRecordUpdateInput, MqSendRecord>();
        }
    }
}
