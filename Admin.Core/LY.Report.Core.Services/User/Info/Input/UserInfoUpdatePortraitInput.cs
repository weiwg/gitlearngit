using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Service.User.Info.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class UserInfoUpdatePortraitInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Required(ErrorMessage = "头像不能为空！")]
        private string _portrait;
        public string Portrait { get => _portrait; set => _portrait = EncryptHelper.Aes.Decrypt(value); }

    }
}
