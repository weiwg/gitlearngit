using LY.Report.Core.LYApiUtil.Pay.Enum;

namespace LY.Report.Core.LYApiUtil.Pay.In
{
    /// <summary>
    /// 查询
    /// </summary>
    public class AccountInfoGetIn
    {
        /// <summary>
        /// 账号Id
        /// 限制为36个字符,选填
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// 用户Id
        /// 限制为36个字符,必填
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 资金平台
        /// 选填
        /// </summary>
        public FundPlatform FundPlatform { get; set; }
    }
}
