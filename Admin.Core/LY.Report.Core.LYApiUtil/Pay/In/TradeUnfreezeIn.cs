
namespace LY.Report.Core.LYApiUtil.Pay.In
{
    /// <summary>
    /// 交易解冻并打款
    /// </summary>
    public class TradeUnfreezeIn
    {
        /// <summary>
        /// 商户单号
        /// 限制为2-64个字符,必填
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 担保交易收款用户Id
        /// 若原交易已存在收款用户Id,则当前字段不生效,以原交易为准
        /// 选填,若下单未传值,则解冻必须传值
        /// 限制为36个字符,选填
        /// </summary>
        public string SecuredTradeUserId { get; set; } = "";

        /// <summary>
        /// App交易手续费(包括平台手续费)
        /// 退款则等比例减少
        /// 默认0,为不修改
        /// 单位元,保留2位小数,大于等于0,选填
        /// </summary>
        public decimal PayAppCharge { get; set; } = 0;
    }
}
