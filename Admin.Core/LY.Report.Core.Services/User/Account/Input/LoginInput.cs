﻿using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.User.Account.Input
{
    /// <summary>
    /// 登录信息
    /// </summary>
    public class LoginInput
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空！")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空！")]
        public string Password { get; set; }
        
        /// <summary>
        /// 验证码
        /// </summary>
        //[Required(ErrorMessage = "验证码不能为空！")]
        public string VerifyCode { get; set; }

        /// <summary>
        /// 验证码键
        /// </summary>
        public string VerifyCodeKey { get; set; }
    }
}
