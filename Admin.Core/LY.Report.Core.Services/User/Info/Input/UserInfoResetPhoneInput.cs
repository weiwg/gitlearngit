using LY.Report.Core.Util.Attributes.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.User.Info.Input
{
    /// <summary>
    /// 重置手机号
    /// </summary>
    public class UserInfoResetPhoneInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage = "用户不能为空！")]
        public string UserId { get; set; }

        /// <summary>
        /// 用户新手机号
        /// </summary>
        [ChinaPhone(ErrorMessage = "手机号不正确！")]
        public string Phone { get; set; }
    }
}
