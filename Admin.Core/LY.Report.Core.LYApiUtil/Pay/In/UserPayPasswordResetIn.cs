
namespace LY.Report.Core.LYApiUtil.Pay.In
{
    /// <summary>
    /// 重置密码
    /// </summary>
    public class UserPayPasswordResetIn
    {
        /// <summary>
        /// 用户Id
        /// 限制为36个字符,必填
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 新密码
        /// Md5混合加密后的密码
        /// 限制为32个字符,必填
        /// </summary>
        public string NewPassword { get; set; }
    }
}
