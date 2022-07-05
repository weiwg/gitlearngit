using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.User.Info.Input
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class UserInfoUpdatePasswordInput
    {
        /// <summary>
        /// 用户旧密码
        /// </summary>
        [Required(ErrorMessage = "旧密码不能为空！"), StringLength(32, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 32)]
        public string OldPassword { get; set; }

        /// <summary>
        /// 用户新密码
        /// </summary>
        [Required(ErrorMessage = "新密码不能为空！"), StringLength(32, ErrorMessage = "新密码限制为{2}-{1} 个字符。", MinimumLength = 32)]
        public string NewPassword { get; set; }

    }
}
