
namespace LY.Report.Core.Service.User.Info.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class UserInfoGetSelectInput
    {
        /// <summary>
        /// 用户名(模糊)
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 昵称(模糊)
        /// </summary>
        public string NickName { get; set; }

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
