using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.Pay.Enum;
using LY.Report.Core.Util.Attributes.Validation;

namespace LY.Report.Core.Service.Pay.Transfer.Input
{
    /// <summary>
    /// 交易转账
    /// </summary>
    public class PayTransferAddInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage = "用户Id不能为空！"), StringLength(36, ErrorMessage = "{0} 限制为{1} 个字符。")]
        public string UserId { get; set; }

        /// <summary>
        /// 商户单号
        /// </summary>
        [Required(ErrorMessage = "商户单号不能为空！"), StringLength(64, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 商户转账单号
        /// </summary>
        [Required(ErrorMessage = "商户转账单号不能为空！"), StringLength(64, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string TransferOutTradeNo { get; set; }

        /// <summary>
        /// 转账类型
        /// </summary>
        [Required(ErrorMessage = "转账类型不能为空！")]
        public TransferType TransferType { get; set; }

        /// <summary>
        /// 转账平台
        /// </summary>
        [Required(ErrorMessage = "转账平台不能为空！")]
        public FundPlatform FundPlatform { get; set; }

        /// <summary>
        /// 转账说明
        /// </summary>
        [Required(ErrorMessage = "转账说明不能为空！"), StringLength(100, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string TransferDescription { get; set; }

        /// <summary>
        /// 转账金额(未扣除手续费的金额)
        /// </summary>
        [CustomPrice]
        [Display(Name = "转账金额")]
        public decimal TransferAmount { get; set; }

        /// <summary>
        /// 转账手续费(手续费若低于支付平台手续费,则会扣除系统余额)(支付宝,微信转账免手续费)
        /// </summary>
        [CustomPrice]
        [Display(Name = "转账手续费")]
        public decimal TransferCharge { get; set; }

        /// <summary>
        /// 收款账号
        /// </summary>
        [Display(Name = "收款账号")]
        [StringLength(100, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string AccountNo { get; set; }

        /// <summary>
        /// 收款姓名
        /// </summary>
        [Display(Name = "收款姓名")]
        [StringLength(100, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string AccountName { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        [Display(Name = "开户行")]
        [StringLength(100, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string BankName { get; set; }

        /// <summary>
        /// 后台通知Url(不可带参数)
        /// </summary>
        [Display(Name = "后台通知Url")]
        [StringLength(256, ErrorMessage = "{0} 限制为{1} 个字符。")]
        public string AppBackNotifyUrl { get; set; }
    }
}
