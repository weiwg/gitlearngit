using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.User.Info.Input
{
    public class UserInfoUpdateAccountInput
    {
        /// <summary>
        /// 新 手机/邮箱
        /// </summary>
        [Required(ErrorMessage = "账号不能为空！")]
        public string NewAccount { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "验证码不能为空！")]
        public string VerifyCode { get; set; }

        /// <summary>
        /// 验证码键
        /// </summary>
        [Required(ErrorMessage = "验证码已失效！")]
        public string VerifyCodeKey { get; set; }

        /// <summary>
        /// 验证凭据键
        /// </summary>
        //[Required(ErrorMessage = "验证凭据键不能为空！")]
        public string VerifyCodeTokenKey { get; set; }
    }
}
