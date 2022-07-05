
namespace LY.Report.Core.LYApiUtil.Pay.In
{
    /// <summary>
    /// 交易退款
    /// </summary>
    public class TradeRefundIn
    {
        /// <summary>
        /// 商户退款单号
        /// 限制为2-64个字符,必填
        /// </summary>
        public string RefundOutTradeNo { get; set; }

        /// <summary>
        /// 商户单号(原单号)
        /// 限制为2-64个字符,必填
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 退款用户Id
        /// 指定退款收款账号,退款到余额
        /// 仅PayOrderType.Order有效
        /// 限制为36个字符,选填
        /// </summary>
        public string RefundUserId { get; set; }

        /// <summary>
        /// 退款金额
        /// (未扣除手续费的金额)
        /// RefundAmount,RefundAppSubsidyAmount一般按支付比例退款
        /// 单位元,保留2位小数,大于等于0,必填
        /// </summary>
        public decimal RefundAmount { get; set; }

        /// <summary>
        /// 退款说明
        /// 限制为2-100个字符,必填
        /// </summary>
        public string RefundDescription { get; set; }

        /// <summary>
        /// 退款系统补贴金额
        /// (未扣除手续费的金额) 
        /// 交易结束,且收款方为系统,此字段无效
        /// RefundAmount,RefundAppSubsidyAmount一般按支付比例退款
        /// 单位元,保留2位小数,大于等于0,必填
        /// </summary>
        public decimal RefundAppSubsidyAmount { get; set; }

        /// <summary>
        /// 退款手续费
        /// (支付宝退款需扣手续费,若app不扣除,则会扣除当前app上的余额造成损失)
        /// 单位元,保留2位小数,大于等于0,必填
        /// </summary>
        public decimal RefundCharge { get; set; }

        /// <summary>
        /// 后台通知Url
        /// 限制为256个字符,不可带参数,选填
        /// </summary>
        public string AppBackNotifyUrl { get; set; }
    }
}
