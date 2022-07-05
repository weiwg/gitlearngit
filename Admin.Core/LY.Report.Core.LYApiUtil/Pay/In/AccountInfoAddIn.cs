using LY.Report.Core.LYApiUtil.Pay.Enum;

namespace LY.Report.Core.LYApiUtil.Pay.In
{
    /// <summary>
    /// 添加
    /// </summary>
    public class AccountInfoAddIn
    {
        /// <summary>
        /// 用户Id
        /// 限制为36个字符,必填
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 资金平台
        /// 必填
        /// </summary>
        public FundPlatform FundPlatform { get; set; }

        /// <summary>
        /// 账号
        /// 限制为2-100个字符,必填
        /// </summary>
        public string AccountNo { get; set; }

        /// <summary>
        /// 姓名
        /// 限制为2-50个字符,必填
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 开户行
        /// 限制为2-100个字符,选填填
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 备注
        /// 限制为100个字符,选填填
        /// </summary>
        public string Remark { get; set; }
    }
}
