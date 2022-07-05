using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.User.Info.Input
{
    public class UserInfoResetPayPasswordInput
    {
        /// <summary>
        /// 新密码
        /// Md5混合加密后的密码
        /// </summary>

        [Required(ErrorMessage = "新密码不能为空！"), StringLength(32, ErrorMessage = "{0} 限制为 {1} 个字符。")]
        public string NewPassword { get; set; }

        /// <summary>
        /// 验证凭据
        /// </summary>
        [Required(ErrorMessage = "验证凭据不能为空！")]
        public string VerifyCodeTokenKey { get; set; }
    }
}
