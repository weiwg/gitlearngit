using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Business.Mq.Input
{
    /// <summary>
    /// 推送消息
    /// </summary>
    public class MqSendMessageIn
    {
        /// <summary>
        /// 模块
        /// </summary>
        [Required(ErrorMessage = "模块不能为空！")]
        public string MsgAction { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Required(ErrorMessage = "内容不能为空！")]
        public string MsgContent { get; set; }

        /// <summary>
        /// 功能名
        /// </summary>
        [Required(ErrorMessage = "功能名不能为空！")]
        public string FuncName { get; set; }

        /// <summary>
        /// 接收系统名
        /// </summary>
        [Required(ErrorMessage = "接收系统名不能为空！")]
        public string ReceiveSysName { get; set; }

        /// <summary>
        /// 发送系统名
        /// </summary>
        [Required(ErrorMessage = "发送系统名不能为空！")]
        public string SendSysName { get; set; } = "delivery";
    }
}
