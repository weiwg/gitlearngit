using System;
using LY.Report.Core.LYApiUtil.Pay.Enum;

namespace LY.Report.Core.LYApiUtil.Pay.In
{
    /// <summary>
    /// 交易下单
    /// </summary>
    public class TradeIn
    {
        /// <summary>
        /// 用户Id
        /// 限制为36个字符,必填
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 商户单号
        /// 限制为2-64个字符,必填
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 支付订单类型
        /// 必填
        /// </summary>
        public PayOrderType PayOrderType { get; set; }

        /// <summary>
        /// 订单描述
        /// 限制为2-100个字符,必填
        /// </summary>
        public string PayDescription { get; set; }

        /// <summary>
        /// 支付金额
        /// 单位元,保留2位小数,大于0,必填
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 系统补贴金额
        /// 系统优惠券,系统红包,积分抵扣等,系统补贴给收款用户
        /// 仅PayOrderType.Order,PayOrderType.Recharge有效
        /// </summary>
        public decimal AppSubsidyAmount { get; set; }

        /// <summary>
        /// App交易结算手续费
        /// (需扣除交易平台手续费,若app不扣除,则会扣除当前app的余额造成损失)
        /// 退款则等比例减少
        /// 余额充值0手续费
        /// 必填
        /// </summary>
        public decimal PayAppCharge { get; set; }

        /// <summary>
        /// 支付过期时间
        /// 必填
        /// 最小5分钟,最大3天,范围外以最值为准
        /// </summary>
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 是否担保交易(资金将冻结,直到发指令解冻)
        /// 默认false,资金直接到账app系统
        /// </summary>
        public bool IsSecuredTrade { get; set; }

        /// <summary>
        /// 担保交易收款用户Id
        /// 资金将冻结,直到发指令解冻
        /// 选填,若下单未传值,则解冻必须传值
        /// </summary>
        public string SecuredTradeUserId { get; set; } = "";

        /// <summary>
        /// 自动付款(交易直接支付)
        /// 默认为空,只支持余额,参数:balance
        /// 下单成功,会下发支付成功回调
        /// 为空则普通交易
        /// </summary>
        public string AutoPay { get; set; } = "";

        /// <summary>
        /// 前台通知Url
        /// 限制为256个字符,不可带参数,选填
        /// </summary>
        public string AppFrontNotifyUrl { get; set; }

        /// <summary>
        /// 后台通知Url
        /// 限制为256个字符,不可带参数,选填
        /// </summary>
        public string AppBackNotifyUrl { get; set; }

        /// <summary>
        /// 取消回调Url
        /// 限制为256个字符,不可带参数,选填
        /// </summary>
        public string AppQuitUrl { get; set; }
    }
}
