using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.User.Info.Input
{
    /// <summary>
    /// 重置密码
    /// </summary>
    public class UserInfoResetPasswordInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage = "用户不能为空！")]
        public string UserId { get; set; }

        /// <summary>
        /// 用户新密码
        /// </summary>
        [Required(ErrorMessage = "新密码不能为空！"), StringLength(32, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 32)]
        public string NewPassword { get; set; }
    }
}
