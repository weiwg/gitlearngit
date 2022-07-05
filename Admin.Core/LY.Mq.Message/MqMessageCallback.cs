using System;
using System.ComponentModel.DataAnnotations;

namespace LY.Mq.Message
{
    /// <summary>
    /// mq消息回调
    /// </summary>
    [Serializable]
    public class MqMessageCallback
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        [Display(Name = "消息Id")]
        public string MsgId { get; set; }

        /// <summary>
        /// 回调消息结果
        /// </summary>
        [Display(Name = "回调消息结果")]
        public string Result { get; set; }

    }
}
