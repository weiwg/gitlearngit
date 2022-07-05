using LY.Report.Core.Common.Captcha.Dtos;
using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.User.Account.Input
{
    /// <summary>
    /// 统一登录信息
    /// </summary>
    public class LYUaLoginInput : LoginInput
    {
        /// <summary>
        /// 统一登录登录Token
        /// </summary>
        //[Required(ErrorMessage = "登录令牌不能为空！")]
        public string LoginToken { get; set; }

        /// <summary>
        /// 身份Id
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 验证数据
        /// </summary>
        public CaptchaInput Captcha { get; set; }

        /// <summary>
        /// 微信OpenId(当前微信浏览器的OpenId,不一定是登录用户的OpenId)
        /// </summary>
        public string WeChatOpenId { get; set; }
    }
}
