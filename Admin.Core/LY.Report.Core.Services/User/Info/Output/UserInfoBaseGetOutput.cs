using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Service.User.Info.Output
{
    /// <summary>
    /// 用户基础信息
    /// </summary>
    public class UserInfoBaseGetOutput
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>

        private string _portrait;
        public string Portrait { get => _portrait.IsNull() ? "" : EncryptHelper.Aes.Encrypt(_portrait); set => _portrait = value; }

        /// <summary>
        /// 头像Url
        /// </summary>
        public string PortraitUrl { get => _portrait; }

        /// <summary>
        /// 邮箱
        /// </summary>
        private string _email;
        public string Email { get => CommonHelper.StringEncryptEmail(_email); set => _email = value; }

        /// <summary>
        /// 手机
        /// </summary>
        private string _phone;
        public string Phone { get => CommonHelper.StringEncryptPhone(_phone); set => _phone = value; }

        /// <summary>
        /// 微信昵称
        /// </summary>
        public string WeChatNickName { get; set; }

    }
}
