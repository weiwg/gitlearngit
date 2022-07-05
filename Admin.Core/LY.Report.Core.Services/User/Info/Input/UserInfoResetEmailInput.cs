using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.User.Info.Input
{
    public class UserInfoResetEmailInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage = "用户不能为空！")]
        public string UserId { get; set; }

        /// <summary>
        /// 用户新手机号
        /// </summary>
        [EmailAddress(ErrorMessage = "邮箱号不正确！")]
        public string Email { get; set; }
    }
}
