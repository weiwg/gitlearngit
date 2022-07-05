
using System.ComponentModel.DataAnnotations;
using LY.Report.Core.LYApiUtil.Pay.Enum;
using LY.Report.Core.Util.Attributes.Validation;

namespace LY.Report.Core.Service.Pay.UaTrade.Input
{
    /// <summary>
    /// 余额提现
    /// </summary>
    public class WithdrawAddInput
    {
        /// <summary>
        /// 提现金额
        /// </summary>
        [Display(Name = "提现金额")]
        [CustomPrice]
        public decimal WithdrawAmount { get; set; }

        /// <summary>
        /// 资金平台
        /// </summary>
        [Required(ErrorMessage = "资金平台不能为空！")]
        public FundPlatform FundPlatform { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "账号不能为空！"), StringLength(100, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string AccountNo { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [StringLength(50, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string AccountName { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        [Display(Name = "开户行")]
        [StringLength(100, ErrorMessage = "{0} 限制为{1} 个字符。")]
        public string BankName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空！"), StringLength(32, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 32)]
        public string PayPassword { get; set; }
    }
}
