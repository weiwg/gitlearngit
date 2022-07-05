using System;
using System.ComponentModel;

namespace LY.Report.Core.Model.Pay.Enum
{
    /// <summary>
    /// 资金平台
    /// </summary>
    [Serializable]
    public enum FundPlatform
    {
        /// <summary>
        /// 银联(银行卡)
        /// </summary>
        [Description("银联")]
        Unionpay = 1,
        /// <summary>
        /// 银联企业网银
        /// </summary>
        [Description("银联企业网银")]
        UnionpayB2B = 11,
        /// <summary>
        /// 支付宝
        /// </summary>
        [Description("支付宝")]
        Alipay = 2,
        /// <summary>
        /// 微信
        /// </summary>
        [Description("微信")]
        WeChat = 3,
        /// <summary>
        /// 余额
        /// </summary>
        [Description("余额")]
        Balance = 4
    }

    /// <summary>
    /// 支付订单类型
    /// </summary>
    [Serializable]
    public enum PayOrderType
    {
        /// <summary>
        /// 支付
        /// </summary>
        [Description("商品订单")]
        Order = 1,
        /// <summary>
        /// 余额充值
        /// </summary>
        [Description("余额充值")]
        Recharge = 2,
        /// <summary>
        /// 保证金
        /// </summary>
        [Description("保证金")]
        Deposit = 3
    }

    /// <summary>
    /// 支付状态
    /// </summary>
    [Serializable]
    public enum PayStatus
    {
        /// <summary>
        /// 待支付
        /// </summary>
        [Description("待支付")]
        Unpaid = 1,
        /// <summary>
        /// 已支付
        /// </summary>
        [Description("已支付")]
        Paid = 2,
        /// <summary>
        /// 已关闭
        /// </summary>
        [Description("已关闭")]
        Closed = 3,
        /// <summary>
        /// 支付失败
        /// </summary>
        [Description("支付失败")]
        Failed = 4,
        /// <summary>
        /// 支付中(除明确状态外的其他状态)
        /// </summary>
        [Description("支付中")]
        Paying = 9
    }

    /// <summary>
    /// 支付宝交易状态,该状态值与PayStatus一致
    /// </summary>
    [Serializable]
    public enum AlipayTradeStatus
    {
        /// <summary>
        /// 交易创建，等待买家付款
        /// </summary>
        [Description("待支付")]
        WAIT_BUYER_PAY = 1,
        /// <summary>
        /// 交易支付成功
        /// </summary>
        [Description("已支付")]
        TRADE_SUCCESS = 2,
        /// <summary>
        /// 交易结束，不可退款
        /// </summary>
        [Description("已支付")]
        TRADE_FINISHED = 2,
        /// <summary>
        /// 未付款交易超时关闭，或支付完成后全额退款
        /// </summary>
        [Description("已关闭")]
        TRADE_CLOSED = 3
    }

    /// <summary>
    /// 担保交易状态
    /// </summary>
    [Serializable]
    public enum SecuredTradeStatus
    {
        /// <summary>
        /// 非担保交易
        /// </summary>
        [Description("非担保交易")]
        Normal = 1,
        /// <summary>
        /// 交易中
        /// </summary>
        [Description("交易中")]
        Trading = 2,
        /// <summary>
        /// 已完成(交易成功)
        /// </summary>
        [Description("已完成")]
        Finish = 4,
        /// <summary>
        /// 已关闭(交易退款/关闭)
        /// </summary>
        [Description("已关闭")]
        Closed = 9
    }

    /// <summary>
    /// 回调状态
    /// </summary>
    public enum CallBack
    {
        /// <summary>
        /// 未回调
        /// </summary>
        [Description("未回调")]
        NotCall = 1,
        /// <summary>
        /// 回调成功
        /// </summary>
        [Description("已回调")]
        CallSuccess = 2,
        /// <summary>
        /// 回调失败
        /// </summary>
        [Description("回调失败")]
        CallFail = 3
    }
}
