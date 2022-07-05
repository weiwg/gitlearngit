
namespace LY.Report.Core.Service.User.Account.Input
{
    /// <summary>
    /// 检查账号信息
    /// </summary>
    public class CheckAcountInput
    { 
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

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
