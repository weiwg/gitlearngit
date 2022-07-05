using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Mq.ReceiveRecord.Input;

namespace LY.Report.Core.Service.Mq.ReceiveRecord
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IMqReceiveRecordService : IBaseService, IAddService<MqReceiveRecordAddInput>, IUpdateService<MqReceiveRecordUpdateInput>, IGetService<MqReceiveRecordGetInput>, ISoftDeleteFullService<MqReceiveRecordDeleteInput>
    {
    }
}
