using System;
using System.ComponentModel.DataAnnotations;

namespace LY.Mq.Message
{
    /// <summary>
    /// mq消息
    /// </summary>
    [Serializable]
    public class MqMessage
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        [Display(Name = "消息Id")]
        public string MsgId { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        [Display(Name = "路由")]
        public string Exchange { get; set; }

        /// <summary>
        /// 队列
        /// </summary>
        [Display(Name = "队列")]
        public string Queue { get; set; }

        /// <summary>
        /// 路由值
        /// </summary>
        [Display(Name = "路由值")]
        public string RoutingKey { get; set; }

        /// <summary>
        /// 消息模块
        /// </summary>
        [Display(Name = "消息模块")]
        public string MsgAction { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [Display(Name = "消息内容")]
        public string MsgContent { get; set; } 

        /// <summary>
        /// 消息时间
        /// </summary>
        [Display(Name = "消息时间")]
        public DateTime MsgDate { get; set; }

        /// <summary>
        /// 回调路由
        /// </summary>
        [Display(Name = "回调路由")]
        public string CallbackExchange { get; set; }

        /// <summary>
        /// 回调路由值
        /// </summary>
        [Display(Name = "回调路由值")]
        public string CallbackRoutingKey { get; set; }

    }
}
