using LY.Report.Core.LYApiUtil.Pay.Enum;

namespace LY.Report.Core.LYApiUtil.Pay.In
{
    /// <summary>
    /// 交易转账
    /// </summary>
    public class TransferIn
    {
        /// <summary>
        /// 用户Id
        /// 限制为36个字符,必填
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 商户转账单号
        /// 限制为2-64个字符,必填
        /// </summary>
        public string TransferOutTradeNo { get; set; }

        /// <summary>
        /// 转账类型
        /// 必填
        /// </summary>
        public TransferType TransferType { get; set; }

        /// <summary>
        /// 转账平台
        /// 必填
        /// </summary>
        public FundPlatform FundPlatform { get; set; }

        /// <summary>
        /// 转账说明
        /// 限制为2-100个字符,必填
        /// </summary>
        public string TransferDescription { get; set; }

        /// <summary>
        /// 转账金额(未扣除手续费的金额)
        /// 单位元,保留2位小数,大于0,必填
        /// </summary>
        public decimal TransferAmount { get; set; }

        /// <summary>
        /// App转账手续费(包括平台手续费)
        /// (手续费若低于支付平台手续费,则会扣除系统余额)(支付宝,微信转账免手续费)
        /// 单位元,保留2位小数,大于0,必填
        /// </summary>
        public decimal TransferAppCharge { get; set; }

        /// <summary>
        /// 收款账号
        /// 限制为2-100个字符,必填
        /// </summary>
        public string AccountNo { get; set; }

        /// <summary>
        /// 收款姓名
        /// 限制为2-100个字符,必填
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 开户行
        /// 限制为2-100个字符,选填
        /// </summary>
        public string BankName { get; set; }

        /// <summary>
        /// 后台通知Url(不可带参数)
        /// 限制为256个字符,不可带参数,选填
        /// </summary>
        public string AppBackNotifyUrl { get; set; }
    }
}
