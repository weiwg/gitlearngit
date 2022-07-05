using System.Threading.Tasks;
using LY.Report.Core.Business.Mq.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Mq.Enum;
using LY.Mq.Message;

namespace LY.Report.Core.Business.Mq
{
    public interface IMqBusiness
    {
        #region 推送

        #region 发布订阅

        #region json
        /// <summary>
        /// 发布订阅
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isCallback">是否需要回调确认</param>
        /// <param name="sendNow">是否立刻推送</param>
        /// <returns></returns>
        Task<IResponseOutput> PublishJsonMsgAsync(MqSendMessageIn input, bool isCallback = false, bool sendNow = false);

        /// <summary>
        /// 发布订阅
        /// </summary>
        /// <param name="content"></param>
        /// <param name="action"></param>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        /// <param name="callbackExchange"></param>
        /// <param name="callbackRoutingKey"></param>
        /// <param name="sendNow">是否立刻推送</param>
        /// <returns></returns>
        Task<IResponseOutput> PublishJsonMsgAsync(string content, string action, string exchangeName, string routingKey, string callbackExchange = "", string callbackRoutingKey = "", bool sendNow = false);

        /// <summary>
        /// 发布订阅
        /// </summary>
        /// <param name="content"></param>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        Task<IResponseOutput> PublishMsgAsync(string content, string exchangeName, string routingKey);
        #endregion

        #region entity
        /// <summary>
        /// 发布订阅
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isCallback">是否需要回调确认</param>
        /// <param name="sendNow">是否立刻推送</param>
        /// <returns></returns>
        Task<IResponseOutput> PublishEntityMsgAsync(MqSendMessageIn input, bool isCallback = false, bool sendNow = false);

        /// <summary>
        /// 发布订阅
        /// </summary>
        /// <param name="content"></param>
        /// <param name="action"></param>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        /// <param name="callbackExchange"></param>
        /// <param name="callbackRoutingKey"></param>
        /// <param name="sendNow">是否立刻推送</param>
        /// <returns></returns>
        Task<IResponseOutput> PublishEntityMsgAsync(string content, string action, string exchangeName, string routingKey, string callbackExchange = "", string callbackRoutingKey = "", bool sendNow = false);

        #endregion
        #endregion

        #region 发送接收

        /// <summary>
        /// 发送接收
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> SendQueueEntityMsgAsync(MqSendMessageIn input);

        /// <summary>
        /// 发送接收
        /// </summary>
        /// <param name="content"></param>
        /// <param name="action"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        Task<IResponseOutput> SendQueueEntityMsgAsync(string content, string action, string queue);
        #endregion

        #endregion

        #region 推送 添加/修改
        /// <summary>
        /// 添加发送记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddMqSendRecordAsync(MqMessage input);

        /// <summary>
        /// 修改发送状态
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="msgHandleStatus"></param>
        /// <param name="msgResult"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateMqSendStatusAsync(string msgId, MqMsgStatus msgHandleStatus, string msgResult);

        /// <summary>
        /// 修改发送回调状态
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="msgStatus"></param>
        /// <param name="msgResult"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateMqSendCallBackStatusAsync(string msgId, MqMsgStatus msgStatus, string msgResult);
        #endregion

        #region 接收 添加/修改
        /// <summary>
        /// 添加接收记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddMqReceiveRecordAsync(MqMessage input);

        /// <summary>
        /// 修改接收状态
        /// </summary>
        /// <param name="recordId"></param>
        /// <param name="msgHandleStatus"></param>
        /// <param name="msgResult"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateMqReceiveStatusAsync(string recordId, MqMsgStatus msgHandleStatus, string msgResult);

        /// <summary>
        /// 修改接收回调状态
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="msgStatus"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateMqReceiveCallBackStatusAsync(string msgId, MqMsgStatus msgStatus);

        #endregion


        #region 回调处理

        /// <summary>
        /// 发布接收状态(消费者返回确认消费到生产者)
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        Task<IResponseOutput> HandleCallBackAsync(string msgId, string exchangeName, string routingKey);
        #endregion
    }
}
