using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Mq.SendRecord.Input;

namespace LY.Report.Core.Service.Mq.SendRecord
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IMqSendRecordService : IBaseService, IAddService<MqSendRecordAddInput>, IUpdateService<MqSendRecordUpdateInput>, IGetService<MqSendRecordGetInput>, ISoftDeleteFullService<MqSendRecordDeleteInput>
    {
    }
}
