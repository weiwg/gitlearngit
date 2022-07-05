using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.User.Info.Input
{
    public class UserInfoUpdatePayPasswordInput
    {
        /// <summary>
        /// 旧密码
        /// Md5混合加密后的密码
        /// </summary>
        [Required(ErrorMessage = "旧密码不能为空！"), StringLength(32, ErrorMessage = "{0} 限制为 {1} 个字符。")]
        public string OldPassword { get; set; }

        /// <summary>
        /// 新密码
        /// Md5混合加密后的密码
        /// </summary>
        [Required(ErrorMessage = "新密码不能为空！"), StringLength(32, ErrorMessage = "{0} 限制为 {1} 个字符。")]
        public string NewPassword { get; set; }
    }
}
