using System;
using LY.Report.Core.Model.Mq.Enum;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Service.Mq.SendRecord.Output
{
    public class MqSendRecordGetOutput
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        public string MsgId { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 队列
        /// </summary>
        public string Queue { get; set; }

        /// <summary>
        /// 路由值
        /// </summary>
        public string RoutingKey { get; set; }

        /// <summary>
        /// 消息模块
        /// </summary>
        public string MsgAction { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MsgContent { get; set; }

        /// <summary>
        /// 消息时间
        /// </summary>
        public DateTime MsgDate { get; set; }

        /// <summary>
        /// 消息状态
        /// </summary>
        public MqMsgStatus MsgStatus { get; set; }

        /// <summary>
        /// 消息状态描述
        /// </summary>
        public string MsgStatusDescribe => EnumHelper.GetDescription(MsgStatus);

        /// <summary>
        /// 消息结果
        /// </summary>
        public string MsgResult { get; set; }

    }
}
