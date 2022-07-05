using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Util.Attributes.Validation;

namespace LY.Report.Core.Service.Pay.Refund.Input
{
    /// <summary>
    /// 交易退款
    /// </summary>
    public class PayRefundAddInput
    {
        /// <summary>
        /// 商户退款单号
        /// </summary>
        [Required(ErrorMessage = "商户退款单号不能为空！"),
         StringLength(64, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string RefundOutTradeNo { get; set; }

        /// <summary>
        /// 商户单号(原单号)
        /// </summary>
        [Required(ErrorMessage = "商户单号不能为空！"),
         StringLength(64, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 退款说明
        /// </summary>
        [Required(ErrorMessage = "退款说明不能为空！"),
         StringLength(100, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string RefundDescription { get; set; }

        /// <summary>
        /// 退款金额(未扣除手续费的金额)
        /// </summary>
        [CustomPrice]
        [Display(Name = "退款金额")]
        public decimal RefundAmount { get; set; }

        /// <summary>
        /// 退款手续费(支付宝退款需扣手续费,若app不扣除,则会扣除当前app上的余额造成损失)
        /// </summary>
        [CustomPrice]
        [Display(Name = "退款手续费")]
        public decimal RefundCharge { get; set; }

        /// <summary>
        /// 后台通知Url(不可带参数)
        /// </summary>
        [Display(Name = "后台通知Url")]
        [StringLength(256, ErrorMessage = "{0} 限制为{1} 个字符。")]
        public string AppBackNotifyUrl { get; set; }
    }
}
