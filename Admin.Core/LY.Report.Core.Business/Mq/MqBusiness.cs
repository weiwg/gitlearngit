using System;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EasyNetQ;
using EasyNetQ.Topology;
using LY.Report.Core.Business.Mq.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Mq;
using LY.Report.Core.Model.Mq.Enum;
using LY.Report.Core.Repository.Mq;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Func;
using LY.Report.Core.Util.Tool;
using LY.Mq.Message;

namespace LY.Report.Core.Business.Mq
{
    public class MqBusiness : IMqBusiness
    {
        private readonly IMapper _mapper;
        private readonly IBus _bus;
        private readonly IMqSendRecordRepository _mqSendRecordRepository;
        private readonly IMqReceiveRecordRepository _mqReceiveRecordRepository;
        private readonly LogHelper _logger = new LogHelper("MqBusiness");

        public MqBusiness(IMapper mapper, IBus bus, IMqSendRecordRepository mqSendRecordRepository, IMqReceiveRecordRepository mqReceiveRecordRepository)
        {
            _mapper = mapper;
            _bus = bus;
            _mqSendRecordRepository = mqSendRecordRepository;
            _mqReceiveRecordRepository = mqReceiveRecordRepository;
        }

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
        public async Task<IResponseOutput> PublishJsonMsgAsync(MqSendMessageIn input, bool isCallback = false, bool sendNow = false)
        {
            var exchangeName = MqFunc.GetExchangeName(input.SendSysName, input.FuncName);
            var routingKey = MqFunc.GetRoutingKey(input.SendSysName, input.ReceiveSysName);
            var callbackExchange = isCallback ? MqFunc.GetExchangeName(input.SendSysName, "callback") : "";
            var callbackRoutingKey = isCallback ? MqFunc.GetRoutingKey(input.SendSysName) : "";
            return await PublishJsonMsgAsync(input.MsgContent, input.MsgAction, exchangeName, routingKey, callbackExchange, callbackRoutingKey, sendNow);
        }

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
        public async Task<IResponseOutput> PublishJsonMsgAsync(string content, string action, string exchangeName, string routingKey, string callbackExchange = "", string callbackRoutingKey = "", bool sendNow = false)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(action) || string.IsNullOrEmpty(exchangeName) || string.IsNullOrEmpty(routingKey))
            {
                return ResponseOutput.NotOk("参数错误");
            }

            var mqMessage = new MqMessage
            {
                MsgId = CommonHelper.GetGuidD,
                Exchange = exchangeName,
                Queue = "",
                RoutingKey = routingKey,
                MsgContent = content,
                MsgAction = action,
                MsgDate = DateTime.Now,
                CallbackExchange = callbackExchange,
                CallbackRoutingKey = callbackRoutingKey
            };
            var res = await AddMqSendRecordAsync(mqMessage);
            if (!res.Success)
            {
                return res;
            }

            if (!sendNow)
            {
                return ResponseOutput.Ok("succ");
            }

            var jsonMsg = NtsJsonHelper.GetJsonStr(mqMessage);
            res = await PublishMsgAsync(jsonMsg, exchangeName, routingKey);
            if (!res.Success)
            {
                await UpdateMqSendStatusAsync(mqMessage.MsgId, MqMsgStatus.HandleFail, res.Msg);
                return res;
            }

            await UpdateMqSendStatusAsync(mqMessage.MsgId, MqMsgStatus.Handled, "ok");
            return ResponseOutput.Ok("succ");
        }

        /// <summary>
        /// 发布订阅
        /// </summary>
        /// <param name="content"></param>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> PublishMsgAsync(string content, string exchangeName, string routingKey)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(exchangeName) || string.IsNullOrEmpty(routingKey))
            {
                return ResponseOutput.NotOk("参数错误");
            }
            try
            {
                var properties = new MessageProperties();
                var body = Encoding.UTF8.GetBytes(content);
                var exchange = await _bus.Advanced.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic);
                await _bus.Advanced.PublishAsync(exchange, routingKey, false, properties, body);
            }
            catch (Exception e)
            {
                _logger.Error("PublishMsg:" + e.Message);
                return ResponseOutput.NotOk(e.Message);
            }

            return ResponseOutput.Ok("succ");
        }

        #endregion

        #region entity
        /// <summary>
        /// 发布订阅
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isCallback">是否需要回调确认</param>
        /// <param name="sendNow">是否立刻推送</param>
        /// <returns></returns>
        public async Task<IResponseOutput> PublishEntityMsgAsync(MqSendMessageIn input, bool isCallback = false, bool sendNow = false)
        {
            var exchangeName = MqFunc.GetExchangeName(input.SendSysName, input.FuncName);
            var routingKey = MqFunc.GetRoutingKey(input.SendSysName, input.ReceiveSysName);
            var callbackExchange = isCallback ? MqFunc.GetExchangeName(input.SendSysName, "callback") : "";
            var callbackRoutingKey = isCallback ? MqFunc.GetRoutingKey(input.SendSysName) : "";
            return await PublishEntityMsgAsync(input.MsgContent, input.MsgAction, exchangeName, routingKey, callbackExchange, callbackRoutingKey, sendNow);
        }

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
        public async Task<IResponseOutput> PublishEntityMsgAsync(string content, string action, string exchangeName, string routingKey, string callbackExchange = "", string callbackRoutingKey = "", bool sendNow = false)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(action) || string.IsNullOrEmpty(exchangeName) || string.IsNullOrEmpty(routingKey))
            {
                return ResponseOutput.NotOk("参数错误");
            }

            var mqMessage = new MqMessage
            {
                MsgId = CommonHelper.GetGuidD,
                Exchange = exchangeName,
                Queue = "",
                RoutingKey = routingKey,
                MsgContent = content,
                MsgAction = action,
                MsgDate = DateTime.Now,
                CallbackExchange = callbackExchange,
                CallbackRoutingKey = callbackRoutingKey
            };
            var res = await AddMqSendRecordAsync(mqMessage);
            if (!res.Success)
            {
                return res;
            }

            if (!sendNow)
            {
                return ResponseOutput.Ok("succ");
            }

            try
            {
                var message = new Message<MqMessage>(mqMessage);
                var exchange = await _bus.Advanced.ExchangeDeclareAsync(mqMessage.Exchange, ExchangeType.Topic);
                await _bus.Advanced.PublishAsync(exchange, mqMessage.RoutingKey, false, message);
            }
            catch (Exception e)
            {
                await UpdateMqSendStatusAsync(mqMessage.MsgId, MqMsgStatus.HandleFail, e.Message);
                _logger.Error("PublishMsg:" + e.Message);
                return ResponseOutput.NotOk(e.Message);
            }

            await UpdateMqSendStatusAsync(mqMessage.MsgId, MqMsgStatus.Handled, "ok");
            return ResponseOutput.Ok("succ");
        }

        #endregion

        #endregion

        #region 发送接收
        /// <summary>
        /// 发送接收
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> SendQueueEntityMsgAsync(MqSendMessageIn input)
        {
            var queue = MqFunc.GetSendQueueName(input.ReceiveSysName, input.SendSysName, input.FuncName);
            return await SendQueueEntityMsgAsync(input.MsgContent,input.MsgAction, queue);
        }

        /// <summary>
        /// 发送接收
        /// </summary>
        /// <param name="content"></param>
        /// <param name="action"></param>
        /// <param name="queue"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> SendQueueEntityMsgAsync(string content, string action, string queue)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(action) || string.IsNullOrEmpty(queue))
            {
                return ResponseOutput.NotOk("参数错误");
            }
            var mqMessage = new MqMessage
            {
                MsgId = CommonHelper.GetGuidD,
                Exchange = "",
                Queue = queue,
                RoutingKey = "",
                MsgContent = content,
                MsgAction = action,
                MsgDate = DateTime.Now
            };
            var res = await AddMqSendRecordAsync(mqMessage);
            if (!res.Success)
            {
                return res;
            }
            try
            {
                await _bus.SendReceive.SendAsync(mqMessage.Queue, mqMessage);
            }
            catch (Exception e)
            {
                await UpdateMqSendStatusAsync(mqMessage.MsgId, MqMsgStatus.HandleFail, e.Message);
                _logger.Error("SendQueue:" + e.Message);
                return ResponseOutput.NotOk(e.Message);
            }

            await UpdateMqSendStatusAsync(mqMessage.MsgId, MqMsgStatus.Handled, "ok");
            return ResponseOutput.Ok("succ");
        }
        #endregion

        #endregion

        #region 推送 添加/修改
        public async Task<IResponseOutput> AddMqSendRecordAsync(MqMessage mqMessage)
        {
            mqMessage.MsgId = mqMessage.MsgId ?? CommonHelper.GetGuidD;
            var entity = _mapper.Map<MqSendRecord>(mqMessage);
            entity.RecordId = CommonHelper.GetGuidD;
            entity.MsgStatus = MqMsgStatus.NotHandled;
            entity.MsgCallBackStatus = MqMsgStatus.NotHandled;
            entity.CallbackExchange = "";
            entity.CallbackRoutingKey = "";
            var id = (await _mqSendRecordRepository.InsertAsync(entity)).Id;
            if (id.IsNull())
            {
                return ResponseOutput.NotOk("添加失败");
            }

            return ResponseOutput.Ok("succ", mqMessage.MsgId);
        }

        public async Task<IResponseOutput> UpdateMqSendStatusAsync(string msgId, MqMsgStatus msgStatus, string msgResult)
        {
            if (string.IsNullOrEmpty(msgId))
            {
                return ResponseOutput.NotOk("请输入Id");
            }

            int res = await _mqSendRecordRepository.UpdateDiyAsync
                .Set(t => t.MsgStatus, msgStatus)
                .Set(t => t.MsgResult, msgResult)
                .Where(t => t.MsgId == msgId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改失败");
            }
            return ResponseOutput.Ok("修改成功");
        }

        public async Task<IResponseOutput> UpdateMqSendCallBackStatusAsync(string msgId, MqMsgStatus msgStatus, string msgResult)
        {
            if (string.IsNullOrEmpty(msgId))
            {
                return ResponseOutput.NotOk("请输入Id");
            }

            int res = await _mqSendRecordRepository.UpdateDiyAsync
                .Set(t => t.MsgCallBackStatus, msgStatus)
                .Set(t => t.MsgCallBackResult, msgResult)
                .Set(t => t.MsgCallBackDate, DateTime.Now)
                .Where(t => t.MsgId == msgId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改失败");
            }
            return ResponseOutput.Ok("修改成功");
        }
        #endregion

        #region 接收 添加/修改
        public async Task<IResponseOutput> AddMqReceiveRecordAsync(MqMessage mqMessage)
        {
            var receiveRecord = await _mqReceiveRecordRepository.GetOneAsync(t => t.MsgId == mqMessage.MsgId);
            if (receiveRecord != null && receiveRecord.RecordId.IsNotNull())
            {
                return ResponseOutput.Ok("ok");
            }

            var entity = _mapper.Map<MqReceiveRecord>(mqMessage);
            entity.RecordId = CommonHelper.GetGuidD;
            entity.MsgId = entity.MsgId ?? CommonHelper.GetGuidD;
            entity.Exchange = entity.Exchange ?? "";
            entity.Queue = entity.Queue ?? "";
            entity.RoutingKey = entity.RoutingKey ?? "";
            entity.MsgStatus = MqMsgStatus.NotHandled;
            entity.MsgDate = entity.MsgDate == DateTime.MinValue ? DateTime.Now : entity.MsgDate;
            entity.MsgCallBackStatus = MqMsgStatus.NotHandled;
            entity.CallbackExchange = "";
            entity.CallbackRoutingKey = "";
            var id = (await _mqReceiveRecordRepository.InsertAsync(entity)).Id;
            if (id.IsNull())
            {
                return ResponseOutput.NotOk("添加失败");
            }

            return ResponseOutput.Ok("succ", entity.RecordId);
        }

        public async Task<IResponseOutput> UpdateMqReceiveStatusAsync(string recordId, MqMsgStatus msgHandleStatus, string msgResult)
        {
            if (string.IsNullOrEmpty(recordId))
            {
                return ResponseOutput.NotOk("请输入Id");
            }

            int res = await _mqReceiveRecordRepository.UpdateDiyAsync
                .Set(t => t.MsgStatus, msgHandleStatus)
                .Set(t => t.MsgResult, msgResult)
                .Where(t => t.RecordId == recordId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改失败");
            }
            return ResponseOutput.Ok("修改成功");
        }

        public async Task<IResponseOutput> UpdateMqReceiveCallBackStatusAsync(string msgId, MqMsgStatus msgStatus)
        {
            if (string.IsNullOrEmpty(msgId))
            {
                return ResponseOutput.NotOk("请输入Id");
            }

            int res = await _mqReceiveRecordRepository.UpdateDiyAsync
                .Set(t => t.MsgCallBackStatus, msgStatus)
                .Set(t => t.MsgCallBackDate, DateTime.Now)
                .Where(t => t.MsgId == msgId)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改失败");
            }
            return ResponseOutput.Ok("修改成功");
        }
        #endregion

        #region 回调处理

        /// <summary>
        /// 发布接收状态(消费者返回确认消费到生产者)
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="exchangeName"></param>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        public async Task<IResponseOutput> HandleCallBackAsync(string msgId, string exchangeName, string routingKey)
        {
            if (msgId.IsNull() || exchangeName.IsNull() || routingKey.IsNull())
            {
                return ResponseOutput.Ok("succ");
            }

            var messageCallback = new MqMessageCallback { MsgId = msgId, Result = "Received" };
            var jsonMsg = NtsJsonHelper.GetJsonStr(messageCallback);
            var res = await PublishMsgAsync(jsonMsg, exchangeName, routingKey);
            if (res.Success)
            {
                //不判断是否修改成功,待定时轮训重发
                await UpdateMqReceiveCallBackStatusAsync(msgId, MqMsgStatus.Handled);
            }
            if (!res.Success)
            {
                _logger.Error("HandleReceiveAsync:" + res.Msg);
            }
            return res;
        }
        #endregion
    }
}
