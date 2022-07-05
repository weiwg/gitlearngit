using System.Threading.Tasks;
using EasyNetQ.Consumer;
using LY.Report.Core.Service.Base.IService;
using LY.Mq.Message;

namespace LY.Report.Core.Service.Mq.MqService
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IMqService : IBaseService
    {
        /// <summary>
        /// 处理订阅消息(商城)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AckStrategy> ConsumeHandleMallAsync(MqMessage input);

        /// <summary>
        /// 处理订阅消息(支付)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AckStrategy> ConsumeHandlePayGatewayAsync(MqMessage input);

        /// <summary>
        /// 处理订阅消息(Xerp)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AckStrategy> ConsumeHandleXerpAsync(MqMessage input);
        
        /// <summary>
        /// 处理订阅消息(回调)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AckStrategy> ConsumeHandleCallBackAsync(MqMessageCallback input);

        /// <summary>
        /// 处理接收发送消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<AckStrategy> HandleReceiveAsync(MqMessage input);

    }
}
