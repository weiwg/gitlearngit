using System;
using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.Pay.Enum;
using LY.Report.Core.Util.Attributes.Validation;

namespace LY.Report.Core.Service.Pay.Income.Input
{
    /// <summary>
    /// 交易下单
    /// </summary>
    public class PayIncomeAddInput
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
        /// 支付订单类型
        /// </summary>
        [Required(ErrorMessage = "订单类型不能为空！")]
        public PayOrderType PayOrderType { get; set; }

        /// <summary>
        /// 订单描述
        /// </summary>
        [Required(ErrorMessage = "订单描述不能为空！"), StringLength(100, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string PayDescription { get; set; }

        /// <summary>
        /// 支付金额(单位元,保留2位小数)
        /// </summary>
        [CustomPrice]
        [Display(Name = "支付金额")]
        public decimal PayAmount { get; set; }

        /// <summary>
        /// App交易结算手续费
        /// (非余额支付需扣除交易平台手续费,若app不扣除,则会扣除当前app的余额造成损失)
        /// 余额充值0手续费
        /// </summary>
        [CustomPrice]
        [Display(Name = "交易手续费")]
        public decimal PayAppCharge { get; set; }

        /// <summary>
        /// 支付过期时间
        /// </summary>
        [CustomNumber]
        [Display(Name = "支付过期时间")]
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 担保交易收款用户Id
        /// 资金将冻结,直到发指令解冻
        /// 为空则普通交易
        /// </summary>
        public string SecuredTradeUserId { get; set; } = "";

        /// <summary>
        /// 前台通知Url(不可带参数)
        /// </summary>
        [Display(Name = "前台通知Url")]
        [StringLength(256, ErrorMessage = "{0} 限制为{1} 个字符。")]
        public string AppFrontNotifyUrl { get; set; }

        /// <summary>
        /// 后台通知Url(不可带参数)
        /// </summary>
        [Display(Name = "后台通知Url")]
        [StringLength(256, ErrorMessage = "{0} 限制为{1} 个字符。")]
        public string AppBackNotifyUrl { get; set; }

        /// <summary>
        /// 取消回调Url(不可带参数)
        /// </summary>
        [Display(Name = "取消回调Url")]
        [StringLength(256, ErrorMessage = "{0} 限制为{1} 个字符。")]
        public string AppQuitUrl { get; set; }
    }
}
