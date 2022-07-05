
namespace LY.Report.Core.LYApiUtil.Pay.In
{
    /// <summary>
    /// 删除
    /// </summary>
    public class AccountInfoDeleteIn
    {
        /// <summary>
        /// 账号Id
        /// 限制为36个字符,必填
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// 用户Id
        /// 限制为36个字符,必填
        /// </summary>
        public string UserId { get; set; }
    }
}
