using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.User.Info.Output
{
    public class UserInfoGetSelectOutput
    {
        /// <summary>
        /// 用户Id
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
        
        public string Portrait { get; set; }

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

    }
}
