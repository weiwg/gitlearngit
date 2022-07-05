using System;
using LY.Report.Core.LYApiUtil.Pay.Enum;

namespace LY.Report.Core.LYApiUtil.Pay.Out
{
    /// <summary>
    /// 交易查询
    /// </summary>
    public class TradeQueryGetOutput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 商户单号
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 平台单号
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 支付平台
        /// </summary>
        public FundPlatform FundPlatform { get; set; }

        /// <summary>
        /// 支付平台描述
        /// </summary>
        public string FundPlatformDescribe { get; set; }

        /// <summary>
        /// 支付订单类型
        /// </summary>
        public PayOrderType PayOrderType { get; set; }

        /// <summary>
        /// 支付订单类型描述
        /// </summary>
        public string PayOrderTypeDescribe { get; set; }

        /// <summary>
        /// 支付描述
        /// </summary>
        public string PayDescription { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 已退款金额
        /// </summary>
        public decimal RefundedAmount { get; set; }

        /// <summary>
        /// App补贴金额
        /// App补贴给收款用户
        /// </summary>
        public decimal AppSubsidyAmount { get; set; }

        /// <summary>
        /// 已退款App补贴金额
        /// </summary>
        public decimal RefundedAppSubsidyAmount { get; set; }

        /// <summary>
        /// 实际App补贴金额
        /// </summary>
        public decimal ActualSettleAppSubsidyAmount { get; set; }

        /// <summary>
        /// App交易手续费(包括平台手续费)
        /// </summary>
        public decimal PayAppCharge { get; set; }

        /// <summary>
        /// 实际App交易手续费(包括平台手续费)
        /// </summary>
        public decimal ActualSettlePayAppCharge { get; set; }

        /// <summary>
        /// 平台交易手续费, 用于计算支付平台扣除的手续费
        /// </summary>
        public decimal PayPlatformCharge { get; set; }

        /// <summary>
        /// 实际结算平台交易手续费, 用于计算支付平台扣除的手续费
        /// </summary>
        public decimal ActualSettlePayPlatformCharge { get; set; }

        /// <summary>
        /// 实际支付金额(支付平台返回)
        /// </summary>
        public decimal ActualPayAmount { get; set; }

        /// <summary>
        /// 实际结算金额(ActualPayAmount-ActualSettlePayPlatformCharge)
        /// </summary>
        public decimal ActualSettleAmount { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayDate { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public PayStatus PayStatus { get; set; }

        /// <summary>
        /// 支付状态描述
        /// </summary>
        public string PayStatusDescribe { get; set; }

        /// <summary>
        /// 支付状态码
        /// </summary>
        public string PayStatusCode { get; set; }

        /// <summary>
        /// 支付状态消息
        /// </summary>
        public string PayStatusMsg { get; set; }

        /// <summary>
        /// 过期时间(单位为分钟,0表示没有过期)
        /// </summary>
        public DateTime ExpireDate { get; set; }
    }
}
