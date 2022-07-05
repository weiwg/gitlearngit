using System;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Consumer;
using EasyNetQ.Topology;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.Service.Mq.MqService;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Func;
using LY.Report.Core.Util.Middleware;
using LY.Report.Core.Util.Tool;
using LY.Mq.Message;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LY.Report.Core.Helper.Mq
{
    public static class MqServiceExtensions
    {
        private readonly static LogHelper Logger = new LogHelper("MqServiceExtensions");

        /// <summary>
        /// 添加消息队列
        /// </summary>
        /// <param name="services"></param>
        /// <param name="env"></param>
        public static void AddMqAsync(this IServiceCollection services, IHostEnvironment env)
        {
            var appConfig = ConfigHelper.Get<AppConfig>("appconfig", env.EnvironmentName);
            if (!appConfig.MqServer.Enable)
            {
                Logger.Info("AddMqAsync: mqServer is close");
                return;
            }
            //var connectionConfiguration = new ConnectionConfiguration
            //{
            //    Hosts = new List<HostConfiguration>
            //    {
            //        new HostConfiguration
            //        {
            //            Host = "127.0.0.1",
            //            Port = 5672
            //        }
            //    },
            //    Port = 5672,
            //    VirtualHost = "/",
            //    UserName = "LY",
            //    Password = "123456",
            //    PrefetchCount = 2,
            //    Timeout = TimeSpan.FromSeconds(10)
            //};
            //var bus = RabbitHutch.CreateBus(connectionConfiguration, x =>{});
            var bus = RabbitHutch.CreateBus(appConfig.MqServer.Connection);

            services.AddSingleton(bus);

            //await bus.AddListener();
            //var bus = services.BuildServiceProvider().GetService<IBus>();
        }

        #region 监听
        /// <summary>
        /// 监听消息
        /// </summary>
        /// <returns></returns>
        public static async Task AddListener(this IApplicationBuilder app, IHostEnvironment env)
        {
            var appConfig = ConfigHelper.Get<AppConfig>("appconfig", env.EnvironmentName);
            if (!appConfig.MqServer.Enable)
            {
                Logger.Info("AddListener: mqServer is close");
                return;
            }

            Logger.Info("starting AddListener");
            var bus = app.ApplicationServices.GetRequiredService<IBus>();
            //var bus = GetService<IBus>();
            try
            {
                #region 高级订阅
                //订阅商城
                //exchange:eup.mall.topic_json_test
                //queue:eup.delivery.mall.topic_json_queue_test
                //routingKey:#.delivery
                bool isSucc = await bus.AdvancedConsumeListener("mall", "delivery", "msg", ConsumeHandleMallAsync);
                if (!isSucc)
                {
                    Logger.Error("初始化错误,AdvancedConsumeListener:mall_delivery_order");
                }

                //订阅支付
                //exchange:eup.paygateway.topic_json_trade_result
                //queue:eup.delivery.paygateway.topic_json_queue_trade_result
                //routingKey:#.delivery
                isSucc = await bus.AdvancedConsumeListener("paygateway", "delivery", "trade_result", ConsumeHandlePayGatewayAsync);
                if (!isSucc)
                {
                    Logger.Error("初始化错误,AdvancedConsumeListener:paygateway_delivery_trade_result");
                }

                //订阅xerp
                //exchange:eup.xerp.topic_json_order
                //queue:eup.delivery.xerp.topic_json_queue_order
                //routingKey:#.delivery
                isSucc = await bus.AdvancedConsumeListener("xerp", "delivery", "msg", ConsumeHandleMallAsync);
                if (!isSucc)
                {
                    Logger.Error("初始化错误,AdvancedConsumeListener:xerp_delivery_order");
                }

                //订阅:回调
                //exchange:eup.delivery.topic_json_callback
                //queue:eup.delivery.topic_json_queue_callback
                //routingKey:#.delivery
                isSucc = await bus.AdvancedConsumeCallBackListener("delivery", ConsumeHandleCallBackAsync);
                if (!isSucc)
                {
                    Logger.Error("初始化错误,AdvancedConsumeCallBackListener:delivery_callback");
                }
                #endregion

                #region 发送接收
                //测试接收
                //queue:eup.mall.delivery.send_json_queue_test
                //isSucc = await bus.SendReceiveListener<MqMessage>("mall", "delivery", "msg", HandleReceiveAsync);
                //if (!isSucc)
                //{
                //    _logger.Error("初始化错误,SendReceiveListener:mall_delivery_order");
                //}
                #endregion
            }
            catch (Exception e)
            {
                Logger.Error("AddListener:" + e.Message);
            }

            Logger.Info("AddListener finish");
        }

        #region Listener
        /// <summary>
        /// 高级订阅
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="sendSysName"></param>
        /// <param name="receiveSysName"></param>
        /// <param name="funcName"></param>
        /// <param name="onMessage"></param>
        /// <returns></returns>
        private static async Task<bool> AdvancedConsumeListener(this IBus bus, string sendSysName, string receiveSysName, string funcName, Func<byte[], MessageProperties, MessageReceivedInfo, Task<AckStrategy>> onMessage)
        {
            Logger.Info($"AdvancedConsumeListener:{sendSysName}_{receiveSysName}_{funcName}");

            #region 高级订阅
            try
            {
                var advancedBus = bus.Advanced;
                //路由命名规则:eup.系统名.消息类型_业务
                var exchange = await advancedBus.ExchangeDeclareAsync(MqFunc.GetExchangeName(sendSysName, funcName), ExchangeType.Topic);
                //队列命名规则:eup.接收系统名.推送系统名.消息类型队列_业务
                var queueName = await advancedBus.QueueDeclareAsync(MqFunc.GetQueueName(receiveSysName, sendSysName, funcName));
                //路由值命名规则:#.系统名
                advancedBus.Bind(exchange, queueName, MqFunc.GetRoutingKey(receiveSysName));
                advancedBus.Consume(queueName, onMessage);
            }
            catch (Exception e)
            {
                Logger.Error("AdvancedConsumeListener:" + e.Message);
                return false;
            }
            #endregion

            Logger.Info($"AdvancedConsumeListener:{sendSysName}_{receiveSysName}_{funcName} ok");
            return true;
        }

        /// <summary>
        /// 高级订阅 回调
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="receiveSysName"></param>
        /// <param name="funcName"></param>
        /// <param name="onMessage"></param>
        /// <returns></returns>
        private static async Task<bool> AdvancedConsumeCallBackListener(this IBus bus, string receiveSysName, Func<byte[], MessageProperties, MessageReceivedInfo, Task<AckStrategy>> onMessage, string funcName = "callback")
        {
            Logger.Info($"AdvancedConsumeCallBackListener:{receiveSysName}_{funcName}");

            #region 高级订阅
            try
            {
                var advancedBus = bus.Advanced;
                //路由命名规则:eup.系统名.消息类型_业务
                var exchange = await advancedBus.ExchangeDeclareAsync(MqFunc.GetExchangeName(receiveSysName, funcName), ExchangeType.Topic);
                //队列命名规则:eup.接收系统名.推送系统名.消息类型队列_业务
                var queueName = await advancedBus.QueueDeclareAsync(MqFunc.GetCallbackQueueName(receiveSysName, funcName));
                //路由值命名规则:#.系统名
                advancedBus.Bind(exchange, queueName, MqFunc.GetRoutingKey(receiveSysName));
                advancedBus.Consume(queueName, onMessage);
            }
            catch (Exception e)
            {
                Logger.Error("AdvancedConsumeCallBackListener:" + e.Message);
                return false;
            }
            #endregion

            Logger.Info($"AdvancedConsumeCallBackListener:{receiveSysName}_{funcName} ok");
            return true;
        }

        /// <summary>
        /// 发送接收
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bus"></param>
        /// <param name="sendSysName"></param>
        /// <param name="receiveSysName"></param>
        /// <param name="funcName"></param>
        /// <param name="onMessage"></param>
        /// <returns></returns>
        private static async Task<bool> SendReceiveListener<T>(this IBus bus, string sendSysName, string receiveSysName, string funcName, Func<T, Task<AckStrategy>> onMessage)
        {
            Logger.Info($"SendReceiveListener:{sendSysName}_{receiveSysName}_{funcName}");

            #region 发送接收
            try
            {
                //队列命名规则:eup.类型.系统名.类型队列_业务
                await bus.SendReceive.ReceiveAsync(MqFunc.GetSendQueueName(receiveSysName, sendSysName, funcName), x => x
                    .Add(onMessage));
            }
            catch (Exception e)
            {
                Logger.Error("SendReceiveListener:" + e.Message);
                return false;
            }
            #endregion

            Logger.Info($"SendReceiveListener:{sendSysName}_{receiveSysName}_{funcName} ok");
            return true;
        }

        #endregion

        #endregion

        #region 业务处理

        private static T GetService<T>() where T : class
        {
            return HttpService.GetService<T>();
        }

        /// <summary>
        /// 处理订阅(商城)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="properties"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static async Task<AckStrategy> ConsumeHandleMallAsync(byte[] data, MessageProperties properties, MessageReceivedInfo info)
        {
            var msg = ConvertMsgData<MqMessage>(data, out var jsonMsg);
            Logger.Debug("ConsumeHandleMallAsync,Msg:"+ jsonMsg);
            if (string.IsNullOrEmpty(msg.MsgAction))
            {
                //错误消息不处理
                return AckStrategies.NackWithoutRequeue;
            }

            var mqService = GetService<IMqService>();
            try
            {
                return await mqService.ConsumeHandleMallAsync(msg);
            }
            catch
            {
                return AckStrategies.NackWithoutRequeue;
            }

            //return AckStrategies.NackWithoutRequeue;//消费失败,不重发
            //return AckStrategies.NackWithRequeue;//消费失败,重发(返回队头,存在循环问题)
            //return AckStrategies.Ack;//确认消费
        }

        /// <summary>
        /// 处理订阅(支付)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="properties"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static async Task<AckStrategy> ConsumeHandlePayGatewayAsync(byte[] data, MessageProperties properties, MessageReceivedInfo info)
        {
            var msg = ConvertMsgData<MqMessage>(data,out var jsonMsg);
            Logger.Debug("ConsumeHandlePayGatewayAsync,Msg:" + jsonMsg);
            if (string.IsNullOrEmpty(msg.MsgAction))
            {
                //错误消息不处理
                return AckStrategies.NackWithoutRequeue;
            }

            var mqService = GetService<IMqService>();
            try
            {
                return await mqService.ConsumeHandlePayGatewayAsync(msg);
            }
            catch
            {
                return AckStrategies.NackWithoutRequeue;
            }

            //return AckStrategies.NackWithoutRequeue;//消费失败,不重发
            //return AckStrategies.NackWithRequeue;//消费失败,重发(返回队头,存在循环问题)
            //return AckStrategies.Ack;//确认消费
        }

        /// <summary>
        /// 处理订阅(Xerp)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="properties"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static async Task<AckStrategy> ConsumeHandleXerpAsync(byte[] data, MessageProperties properties, MessageReceivedInfo info)
        {
            var msg = ConvertMsgData<MqMessage>(data, out var jsonMsg);
            Logger.Debug("ConsumeHandleXerpAsync,Msg:" + jsonMsg);
            if (string.IsNullOrEmpty(msg.MsgAction))
            {
                //错误消息不处理
                return AckStrategies.NackWithoutRequeue;
            }

            var mqService = GetService<IMqService>();
            try
            {
                return await mqService.ConsumeHandleXerpAsync(msg);
            }
            catch
            {
                return AckStrategies.NackWithoutRequeue;
            }

            //return AckStrategies.NackWithoutRequeue;//消费失败,不重发
            //return AckStrategies.NackWithRequeue;//消费失败,重发(返回队头,存在循环问题)
            //return AckStrategies.Ack;//确认消费
        }

        /// <summary>
        /// 处理订阅(回调消息处理)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="properties"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static async Task<AckStrategy> ConsumeHandleCallBackAsync(byte[] data, MessageProperties properties, MessageReceivedInfo info)
        {
            var msg = ConvertMsgData<MqMessageCallback>(data, out var jsonMsg);
            Logger.Debug("ConsumeHandleCallbackAsync,Msg:" + jsonMsg);
            if (string.IsNullOrEmpty(msg.MsgId))
            {
                //错误消息不处理
                return AckStrategies.NackWithoutRequeue;
            }

            var mqService = GetService<IMqService>();
            try
            {
                return await mqService.ConsumeHandleCallBackAsync(msg);
            }
            catch
            {
                return AckStrategies.NackWithoutRequeue;
            }

            //return AckStrategies.NackWithoutRequeue;//消费失败,不重发
            //return AckStrategies.NackWithRequeue;//消费失败,重发(返回队头,存在循环问题)
            //return AckStrategies.Ack;//确认消费
        }

        /// <summary>
        /// 处理发送接收
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static async Task<AckStrategy> HandleReceiveAsync(MqMessage msg)
        {
            var jsonMsg = NtsJsonHelper.GetJsonStr(msg);
            Logger.Debug("HandleReceiveAsync,Msg:" + jsonMsg);
            if (string.IsNullOrEmpty(msg.MsgAction))
            {
                return AckStrategies.NackWithoutRequeue;
            }

            var mqService = GetService<IMqService>();
            try
            {
                return await mqService.HandleReceiveAsync(msg);
            }
            catch
            {
                return AckStrategies.NackWithoutRequeue;
            }

            //return AckStrategies.NackWithoutRequeue;//消费失败,不重发
            //return AckStrategies.NackWithRequeue;//消费失败,重发(返回队头,存在循环问题)
            //return AckStrategies.Ack;//确认消费
        }

        private static T ConvertMsgData<T>(byte[] data, out string json)
        {
            json = "";
            try
            {
                json = Encoding.UTF8.GetString(data);
            }
            catch
            {
                // ignored
            }
            return NtsJsonHelper.GetJsonEntry<T>(json);
        }

        #endregion
    }
}
