
namespace LY.Report.Core.LYApiUtil.Pay.In
{
    /// <summary>
    /// 关闭交易
    /// </summary>
    public class TradeCloseIn
    {
        /// <summary>
        /// 商户单号
        /// 限制为2-64个字符,必填
        /// 以下情况需要调用关单接口：商户订单支付失败需要生成新单号重新发起支付，要对原订单号调用关单，避免重复支付；系统下单后，用户支付超时，系统退出不再受理，避免用户继续，请调用关单接口。
        /// 注意：订单生成后不能马上调用关单接口，最短调用时间间隔为5分钟。
        /// </summary>
        public string OutTradeNo { get; set; }
    }
}
