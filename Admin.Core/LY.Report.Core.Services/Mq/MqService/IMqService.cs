using System.Threading.Tasks;
using EasyNetQ.Consumer;
using LY.Report.Core.Service.Base.IService;
using LY.Mq.Message;

namespace LY.Report.Core.Service.Mq.MqService
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IMqService : IBaseService
    {
        /// <summary>
        /// ��������Ϣ(�̳�)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AckStrategy> ConsumeHandleMallAsync(MqMessage input);

        /// <summary>
        /// ��������Ϣ(֧��)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AckStrategy> ConsumeHandlePayGatewayAsync(MqMessage input);

        /// <summary>
        /// ��������Ϣ(Xerp)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AckStrategy> ConsumeHandleXerpAsync(MqMessage input);
        
        /// <summary>
        /// ��������Ϣ(�ص�)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AckStrategy> ConsumeHandleCallBackAsync(MqMessageCallback input);

        /// <summary>
        /// ������շ�����Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AckStrategy> HandleReceiveAsync(MqMessage input);

    }
}
