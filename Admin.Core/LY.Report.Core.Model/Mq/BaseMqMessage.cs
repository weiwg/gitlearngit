using System;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Mq
{
    /// <summary>
    /// mq消息基类
    /// </summary>
    public class BaseMqMessage : EntityTenantFull
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        [Description("消息Id")]
        [Column(StringLength = 50, IsNullable = false)]
        public string MsgId { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        [Description("路由")]
        [Column(StringLength = 100, IsNullable = false)]
        public string Exchange { get; set; }

        /// <summary>
        /// 队列
        /// </summary>
        [Description("队列")]
        [Column(StringLength = 100, IsNullable = false)]
        public string Queue { get; set; }

        /// <summary>
        /// 路由值
        /// </summary>
        [Description("路由值")]
        [Column(StringLength = 100, IsNullable = false)]
        public string RoutingKey { get; set; }

        /// <summary>
        /// 消息模块
        /// </summary>
        [Description("消息模块")]
        [Column(StringLength = 100, IsNullable = false)]
        public string MsgAction { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [Description("消息内容")]
        [Column(StringLength = -1, IsNullable = false)]
        public string MsgContent { get; set; } 

        /// <summary>
        /// 消息时间
        /// </summary>
        [Description("消息时间")]
        public DateTime MsgDate { get; set; }

        /// <summary>
        /// 回调路由
        /// </summary>
        [Description("回调路由")]
        [Column(StringLength = 100, IsNullable = false)]
        public string CallbackExchange { get; set; }

        /// <summary>
        /// 回调路由值
        /// </summary>
        [Description("回调路由值")]
        [Column(StringLength = 100, IsNullable = false)]
        public string CallbackRoutingKey { get; set; }


    }
}
