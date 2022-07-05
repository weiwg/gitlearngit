using System;
using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.Mq.Enum;

namespace LY.Report.Core.Service.Mq.SendRecord.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class MqSendRecordAddInput
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        [Display(Name = "消息Id")]
        [Required(ErrorMessage = "消息Id"), StringLength(36, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string MsgId { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        [Display(Name = "路由")]
        [Required(ErrorMessage = "路由"), StringLength(50, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string Exchange { get; set; }

        /// <summary>
        /// 队列
        /// </summary>
        [Display(Name = "队列")]
        [Required(ErrorMessage = "队列"), StringLength(50, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string Queue { get; set; }

        /// <summary>
        /// 路由值
        /// </summary>
        [Display(Name = "路由值")]
        [Required(ErrorMessage = "路由值"), StringLength(50, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string RoutingKey { get; set; }

        /// <summary>
        /// 消息模块
        /// </summary>
        [Display(Name = "消息模块")]
        [Required(ErrorMessage = "消息模块"), StringLength(50, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string MsgAction { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [Display(Name = "消息内容")]
        [Required(ErrorMessage = "路由")]
        public string MsgContent { get; set; }

        /// <summary>
        /// 消息时间
        /// </summary>
        [Display(Name = "消息时间")]
        public DateTime MsgDate { get; set; }

        /// <summary>
        /// 消息状态
        /// </summary>
        [Display(Name = "消息状态")]
        public MqMsgStatus MsgStatus { get; set; }
    }
}
