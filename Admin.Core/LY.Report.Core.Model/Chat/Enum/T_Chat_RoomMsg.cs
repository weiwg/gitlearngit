using System.ComponentModel;

namespace LY.Report.Core.Model.Chat.Enum
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum ChatMsgType
    {
        /// <summary>
        /// 文本
        /// </summary>
        [Description("文本")]
        Text = 1,
        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片")]
        Img = 2,
        /// <summary>
        /// 语音
        /// </summary>
        [Description("语音")]
        Voice = 3,
        /// <summary>
        /// 视频
        /// </summary>
        [Description("视频")]
        Video = 4
    }
}
