using System;
using System.ComponentModel;

namespace LY.Report.Core.Model.Fund.Enum
{
    /// <summary>
    /// 资金记录类型
    /// </summary>
    [Serializable]
    public enum FundRecordType
    {
        /// <summary>
        /// 用户余额账户
        /// </summary>
        [Description("用户余额账户")]
        UserBalance = 110,
        /// <summary>
        /// 用户余额账户(不可提现)
        /// </summary>
        [Description("用户余额账户(不可提现)")]
        UserNoWithdrawBalance = 111,
        /// <summary>
        /// 用户冻结账户
        /// </summary>
        [Description("用户冻结账户")]
        UserFrozenBalance = 120,

        /// <summary>
        /// 系统余额账户
        /// </summary>
        [Description("系统余额账户")]
        AppBalance = 210,
        /// <summary>
        /// 系统冻结账户
        /// </summary>
        [Description("系统冻结账户")]
        AppFrozenBalance = 220
    }

    /// <summary>
    /// 资金类型
    /// 100~199收入+
    /// 200~299支出-
    /// 300~399冻结+
    /// 400~499解冻-
    /// </summary>
    [Serializable]
    public enum FundType
    {
        /// <summary>
        /// 收入
        /// </summary>
        [Description("收入")]
        Income = 100,
        /// <summary>
        /// 订单收入
        /// </summary>
        [Description("订单收入")]
        OrderIncome = 101,
        /// <summary>
        /// 充值
        /// </summary>
        [Description("充值")]
        Recharge = 102,
        /// <summary>
        /// 退款(收入)
        /// </summary>
        [Description("退款(收入)")]
        UserOrderRefund = 103,
        /// <summary>
        /// 订单返现(用户)
        /// </summary>
        [Description("订单返现(用户)")]
        UserOrderCashBack = 112,
        /// <summary>
        /// 提现/转账失败
        /// </summary>
        [Description("提现/转账失败")]
        TransferFail = 121,
        /// <summary>
        /// 保证金转入余额
        /// </summary>
        [Description("保证金转入余额")]
        DepositIncome = 131,

        /// <summary>
        /// 支出
        /// </summary>
        [Description("支出")]
        Expend = 200,
        /// <summary>
        /// 订单退款
        /// </summary>
        [Description("订单退款")]
        OrderRefund = 201,
        /// <summary>
        /// 订单支付
        /// </summary>
        [Description("订单支付")]
        OrderPay = 202,
        /// <summary>
        /// 订单手续费
        /// </summary>
        [Description("订单手续费")]
        OrderCharge = 211,
        /// <summary>
        /// 订单返现(系统)
        /// </summary>
        [Description("订单返现(系统)")]
        AppOrderCashBack = 212,
        /// <summary>
        /// 余额提现/转账
        /// </summary>
        [Description("余额提现/转账")]
        Transfer = 221,


        /// <summary>
        /// 冻结
        /// </summary>
        [Description("冻结")]
        Frozen = 300,
        /// <summary>
        /// 订单收入(订单付款)
        /// </summary>
        [Description("订单收入")]
        FrozenOrderIncome = 301,
        /// <summary>
        /// 保证金冻结
        /// </summary>
        [Description("保证金冻结")]
        FrozenDeposit = 311,
        /// <summary>
        /// 提现/转账冻结
        /// </summary>
        [Description("提现/转账冻结")]
        FrozenTransfer = 321,
        /// <summary>
        /// 解冻
        /// </summary>
        [Description("解冻")]
        Unfreeze = 400,
        /// <summary>
        /// 解冻订单收入(确认收货)
        /// </summary>
        [Description("订单收入")]
        UnfreezeOrderIncome = 401,
        /// <summary>
        /// 解冻订单取消退款
        /// </summary>
        [Description("订单取消退款")]
        UnfreezeOrderCancel = 402,
        /// <summary>
        /// 解冻订单退款
        /// </summary>
        [Description("订单退款")]
        UnfreezeOrderRefund = 403,
        /// <summary>
        /// 保证金解冻
        /// </summary>
        [Description("保证金解冻")]
        UnfreezeDeposit = 411,
        /// <summary>
        /// 提现/转账失败解冻
        /// </summary>
        [Description("提现/转账失败解冻")]
        UnfreezeTransferFail = 421,
        /// <summary>
        /// 提现/转账成功解冻
        /// </summary>
        [Description("提现/转账成功解冻")]
        UnfreezeTransferSuccess = 422
    }
    
}
