using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Util.Attributes.Validation;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Service.User.Info.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class UserInfoUpdateInput 
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
        /// 昵称
        /// </summary>
        [Required(ErrorMessage = "昵称不能为空！"), StringLength(20, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        private string _portrait;
        public string Portrait { get => _portrait; set => _portrait = EncryptHelper.Aes.Decrypt(value); }

    }
}
