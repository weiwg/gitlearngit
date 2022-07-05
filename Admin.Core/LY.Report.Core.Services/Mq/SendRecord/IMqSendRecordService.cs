using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Mq.SendRecord.Input;

namespace LY.Report.Core.Service.Mq.SendRecord
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IMqSendRecordService : IBaseService, IAddService<MqSendRecordAddInput>, IUpdateService<MqSendRecordUpdateInput>, IGetService<MqSendRecordGetInput>, ISoftDeleteFullService<MqSendRecordDeleteInput>
    {
    }
}
