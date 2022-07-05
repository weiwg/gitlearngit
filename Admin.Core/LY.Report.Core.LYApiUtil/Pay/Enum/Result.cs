using System;
using System.ComponentModel;

namespace LY.Report.Core.LYApiUtil.Pay.Enum
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

    #region Trade
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
    #endregion

    #region Refund
    /// <summary>
    /// 退款状态
    /// </summary>
    public enum RefundStatus
    {
        /// <summary>
        /// 待退款
        /// </summary>
        [Description("待退款")]
        Unpaid = 1,
        /// <summary>
        /// 已退款
        /// </summary>
        [Description("已退款")]
        Paid = 2,
        /// <summary>
        /// 已关闭
        /// </summary>
        [Description("已关闭")]
        Closed = 3,
        /// <summary>
        /// 退款失败
        /// </summary>
        [Description("退款失败")]
        Failed = 4,
        /// <summary>
        /// 支付中(除明确状态外的其他状态)
        /// </summary>
        [Description("退款中")]
        Paying = 9
    }
    #endregion

    #region Transfer
    /// <summary>
    /// 转账类型
    /// </summary>
    [Serializable]
    public enum TransferType
    {
        /// <summary>
        /// 提现
        /// </summary>
        [Description("提现")]
        Withdraw = 1,
        /// <summary>
        /// 转账
        /// </summary>
        [Description("转账")]
        Transfer = 2,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 99
    }

    /// <summary>
    /// 内部转账类型
    /// </summary>
    [Serializable]
    public enum TransferInsideType
    {
        /// <summary>
        /// 系统
        /// </summary>
        [Description("系统")]
        App = 1,
        /// <summary>
        /// 用户
        /// </summary>
        [Description("用户")]
        User = 2
    }

    /// <summary>
    /// 转账状态
    /// </summary>
    [Serializable]
    public enum TransferStatus
    {
        /// <summary>
        /// 待转账
        /// </summary>
        [Description("待转账")]
        Unpaid = 1,
        /// <summary>
        /// 已转账(成功)
        /// </summary>
        [Description("已转账")]
        Paid = 2,
        /// <summary>
        /// 已关闭
        /// </summary>
        [Description("已关闭")]
        Closed = 3,
        /// <summary>
        /// 转账失败
        /// </summary>
        [Description("转账失败")]
        Failed = 4,
        /// <summary>
        /// 转账中(除明确状态外的其他状态)
        /// </summary>
        [Description("转账中")]
        Paying = 9
    }
    #endregion


    /// <summary>
    /// 充值状态
    /// </summary>
    [Serializable]
    public enum RechargeStatus
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
        Closed = 3
    }
}
