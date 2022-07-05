

using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.User.Info.Input
{
    public class UserInfResetPasswordAccountInput
    {
        /// <summary>
        /// 验证凭据
        /// </summary>
        public string VerifyCodeTokenKey { get; set; }
        /// <summary>
        /// 新密码
        /// Md5混合加密后的密码
        /// 限制为32个字符,必填
        /// </summary>
        [Required(ErrorMessage = "新密码不能为空！"), StringLength(32, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 32)]
        public string NewPassword { get; set; }
    }
}
