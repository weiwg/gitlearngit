using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.User.Info.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class UserInfoAddInput 
    {
        
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 微信OpenId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空！")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空！"), StringLength(16, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 6)]
        public string Password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Required(ErrorMessage = "昵称不能为空！"), StringLength(18, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Portrait { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

    }
}
