
using LY.Report.Core.LYApiUtil.Pay.Enum;

namespace LY.Report.Core.LYApiUtil.Pay.Out
{
    public class AccountInfoGetOut
    {
        /// <summary>
        /// 账号Id
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// 资金平台
        /// </summary>
        public FundPlatform FundPlatform { get; set; }

        /// <summary>
        /// 资金平台描述
        /// </summary>
        public string FundPlatformDescribe { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string AccountNo { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
