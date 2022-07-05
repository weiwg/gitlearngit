using EasyNetQ.Consumer;
using LY.Mq.Message;
using LY.Report.Core.Business.Mq;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Mq.Enum;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Tool;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Mq.MqService
{
    public class MqService : BaseService, IMqService
    {
        private readonly IMqBusiness _mqBusiness;
        private readonly ILogHelper _logger;

        public MqService(IMqBusiness mqBusiness)
        {
            _logger = new LogHelper("MqService");
            _mqBusiness = mqBusiness;
        }

        public async Task<AckStrategy> ConsumeHandleMallAsync(MqMessage msg)
        {
            if (msg == null || msg.MsgAction.IsNull())
            {
                return AckStrategies.NackWithoutRequeue;
            }

            var res = await _mqBusiness.AddMqReceiveRecordAsync(msg);
            if (!res.Success)
            {
                return AckStrategies.NackWithoutRequeue;
            }
            //���ͻص�״̬,д���¼�ɹ�,�����Ϣ���յ�
            await _mqBusiness.HandleCallBackAsync(msg.MsgId, msg.CallbackExchange, msg.CallbackRoutingKey);

            if (res.Msg == "ok")
            {
                //�ظ���Ϣ,�ص�ȷ�Ϻ��ٴ���
                return AckStrategies.NackWithoutRequeue;
            }

            var recordId = res.GetData<string>();

            res = ResponseOutput.Ok("succ");
            if (!res.Success)
            {
                _logger.Error("�������:" + res.Msg + ",\r\nConsumeHandleMallAsync:" + NtsJsonHelper.GetJsonStr(msg));
            }

            res = await _mqBusiness.UpdateMqReceiveStatusAsync(recordId, res.Success ? MqMsgStatus.Handled : MqMsgStatus.HandleFail, res.Msg);
            if (!res.Success)
            {
                return AckStrategies.NackWithoutRequeue;
            }

            //return AckStrategies.NackWithoutRequeue;//����ʧ��,���ط�
            //return AckStrategies.NackWithRequeue;//����ʧ��,�ط�(���ض�ͷ,����ѭ������)
            return AckStrategies.Ack;//ȷ������
        }

        public async Task<AckStrategy> ConsumeHandlePayGatewayAsync(MqMessage msg)
        {
            if (msg == null || msg.MsgAction.IsNull())
            {
                return AckStrategies.NackWithoutRequeue;
            }

            var res = await _mqBusiness.AddMqReceiveRecordAsync(msg);
            if (!res.Success)
            {
                return AckStrategies.NackWithoutRequeue;
            }
            //���ͻص�״̬,д���¼�ɹ�,�����Ϣ���յ�
            await _mqBusiness.HandleCallBackAsync(msg.MsgId, msg.CallbackExchange, msg.CallbackRoutingKey);

            if (res.Msg == "ok")
            {
                //�ظ���Ϣ,�ص�ȷ�Ϻ��ٴ���
                return AckStrategies.NackWithoutRequeue;
            }

            var recordId = res.GetData<string>();

            if (!res.Success)
            {
                _logger.Error("�������:" + res.Msg + ",\r\nConsumeHandlePayGatewayAsync:" + NtsJsonHelper.GetJsonStr(msg));
            }

            res = await _mqBusiness.UpdateMqReceiveStatusAsync(recordId, res.Success ? MqMsgStatus.Handled : MqMsgStatus.HandleFail, res.Msg);
            if (!res.Success)
            {
                return AckStrategies.NackWithoutRequeue;
            }

            //return AckStrategies.NackWithoutRequeue;//����ʧ��,���ط�
            //return AckStrategies.NackWithRequeue;//����ʧ��,�ط�(���ض�ͷ,����ѭ������)
            return AckStrategies.Ack;//ȷ������
        }

        public async Task<AckStrategy> ConsumeHandleXerpAsync(MqMessage msg)
        {
            if (msg == null || msg.MsgAction.IsNull())
            {
                return AckStrategies.NackWithoutRequeue;
            }

            var res = await _mqBusiness.AddMqReceiveRecordAsync(msg);
            if (!res.Success)
            {
                return AckStrategies.NackWithoutRequeue;
            }
            //���ͻص�״̬,д���¼�ɹ�,�����Ϣ���յ�
            await _mqBusiness.HandleCallBackAsync(msg.MsgId, msg.CallbackExchange, msg.CallbackRoutingKey);

            if (res.Msg == "ok")
            {
                //�ظ���Ϣ,�ص�ȷ�Ϻ��ٴ���
                return AckStrategies.NackWithoutRequeue;
            }

            var recordId = res.GetData<string>();

            res = ResponseOutput.Ok("succ");
            if (msg.MsgAction == "action")
            {
                //do something
            }
            if (!res.Success)
            {
                _logger.Error("�������:" + res.Msg + ",\r\nConsumeHandleXerpAsync:" + NtsJsonHelper.GetJsonStr(msg));
            }

            res = await _mqBusiness.UpdateMqReceiveStatusAsync(recordId, res.Success ? MqMsgStatus.Handled : MqMsgStatus.HandleFail, res.Msg);
            if (!res.Success)
            {
                return AckStrategies.NackWithoutRequeue;
            }

            //return AckStrategies.NackWithoutRequeue;//����ʧ��,���ط�
            //return AckStrategies.NackWithRequeue;//����ʧ��,�ط�(���ض�ͷ,����ѭ������)
            return AckStrategies.Ack;//ȷ������
        }

        public async Task<AckStrategy> ConsumeHandleCallBackAsync(MqMessageCallback msg)
        {
            if (msg == null || msg.MsgId.IsNull())
            {
                return AckStrategies.NackWithoutRequeue;
            }

            var res = await _mqBusiness.UpdateMqSendCallBackStatusAsync(msg.MsgId, MqMsgStatus.Handled, msg.Result);
            if (!res.Success)
            {
                return AckStrategies.NackWithoutRequeue;
            }

            //return AckStrategies.NackWithoutRequeue;//����ʧ��,���ط�
            //return AckStrategies.NackWithRequeue;//����ʧ��,�ط�(���ض�ͷ,����ѭ������)
            return AckStrategies.Ack;//ȷ������
        }

        public async Task<AckStrategy> HandleReceiveAsync(MqMessage msg)
        {
            if (msg == null || msg.MsgAction.IsNull())
            {
                return AckStrategies.NackWithoutRequeue;
            }

            var res = await _mqBusiness.AddMqReceiveRecordAsync(msg);
            if (!res.Success)
            {
                return AckStrategies.NackWithoutRequeue;
            }
            //���ͻص�״̬,д���¼�ɹ�,�����Ϣ���յ�
            await _mqBusiness.HandleCallBackAsync(msg.MsgId, msg.CallbackExchange, msg.CallbackRoutingKey);

            if (res.Msg == "ok")
            {
                //�ظ���Ϣ,�ص�ȷ�Ϻ��ٴ���
                return AckStrategies.NackWithoutRequeue;
            }

            var recordId = res.GetData<string>();

            res = ResponseOutput.Ok("succ");
            if (msg.MsgAction == "action")
            {
                //do something
            }
            if (!res.Success)
            {
                _logger.Error("�������:" + res.Msg + ",\r\nHandleReceiveAsync:" + NtsJsonHelper.GetJsonStr(msg));
            }

            res = await _mqBusiness.UpdateMqReceiveStatusAsync(recordId, res.Success ? MqMsgStatus.Handled : MqMsgStatus.HandleFail, res.Msg);
            if (!res.Success)
            {
                return AckStrategies.NackWithoutRequeue;
            }

            //return AckStrategies.NackWithoutRequeue;//����ʧ��,���ط�
            //return AckStrategies.NackWithRequeue;//����ʧ��,�ط�(���ض�ͷ,����ѭ������)
            return AckStrategies.Ack;//ȷ������
        }

    }
}
