using System;
using System.ComponentModel;
using FreeSql.DataAnnotations;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Pay.Enum;

namespace LY.Report.Core.Model.Pay
{
    /// <summary>
    /// 支付表
    /// </summary>
    [Table(Name = "T_Pay_Income")]
    [Index("idx_{tablename}_01", "OutTradeNo AES", true)]
    public class PayIncome : EntityTenantFull
    {
        /// <summary>
        /// 支付Id
        /// </summary>
        [Description("支付Id")]
        [Column(Position = 2, StringLength = 36, IsNullable = false)]
        public string PayId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Description("用户Id")]
        [Column(StringLength = 36, IsNullable = false)]
        public string UserId { get; set; }

        /// <summary>
        /// 商户单号
        /// </summary>
        [Description("商户单号")]
        [Column(IsPrimary = true, StringLength = 64, IsNullable = false)]
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 平台单号
        /// </summary>
        [Description("平台单号")]
        [Column(StringLength = 64, IsNullable = false)]
        public string TradeNo { get; set; }

        /// <summary>
        /// 支付平台
        /// </summary>
        [Description("支付平台")]
        public FundPlatform FundPlatform { get; set; }
        
        /// <summary>
        /// 支付订单类型
        /// </summary>
        [Description("支付订单类型")]
        [Column(StringLength = 100, IsNullable = false)]
        public PayOrderType PayOrderType { get; set; }

        /// <summary>
        /// 支付描述
        /// </summary>
        [Description("支付描述")]
        [Column(StringLength = 100, IsNullable = false)]
        public string PayDescription { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        [Description("支付金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 已退款金额
        /// </summary>
        [Description("已退款金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal RefundedAmount { get; set; }

        /// <summary>
        /// 系统补贴金额
        /// 系统优惠券,系统红包,积分抵扣等,系统补贴给收款用户
        /// </summary>
        [Description("系统补贴金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal AppSubsidyAmount { get; set; }

        /// <summary>
        /// 已退款系统补贴金额
        /// </summary>
        [Description("已退款系统补贴金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal RefundedAppSubsidyAmount { get; set; }

        private decimal _payAppCharge;
        /// <summary>
        /// App交易手续费(包括平台手续费)
        /// </summary>
        [Description("交易服务费")]
        [Column(Precision = 12, Scale = 2)]
        public decimal PayAppCharge{get => _payAppCharge;set => _payAppCharge = Math.Round(value, 2, MidpointRounding.AwayFromZero);}

        /// <summary>
        /// 实际App交易手续费(包括平台手续费)
        /// </summary>
        [Description("实际结算平台交易手续费")]
        [Column(Precision = 12, Scale = 2)]
        public decimal ActualSettlePayAppCharge { get; set; }

        private decimal _payPlatformCharge;
        /// <summary>
        /// 平台交易手续费, 用于计算支付平台扣除的手续费
        /// </summary>
        [Description("交易服务费")]
        [Column(Precision = 12, Scale = 2)]
        public decimal PayPlatformCharge { get => _payPlatformCharge; set => _payPlatformCharge = Math.Round(value, 2, MidpointRounding.AwayFromZero); }

        /// <summary>
        /// 实际结算平台交易手续费, 用于计算支付平台扣除的手续费
        /// </summary>
        [Description("实际结算平台交易手续费")]
        [Column(Precision = 12, Scale = 2)]
        public decimal ActualSettlePayPlatformCharge { get; set; }

        /// <summary>
        /// 实际支付金额(支付平台返回)
        /// </summary>
        [Description("实际支付金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal ActualPayAmount { get; set; }

        /// <summary>
        /// 实际结算金额(ActualPayAmount-ActualSettlePayPlatformCharge)
        /// </summary>
        public decimal ActualSettleAmount { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        [Description("支付时间")]
        public DateTime? PayDate { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        [Description("支付状态")]
        public PayStatus PayStatus { get; set; }

        /// <summary>
        /// 支付状态码
        /// </summary>
        [Description("支付状态码")]
        [Column(StringLength = -1)]
        public string PayStatusCode { get; set; }

        /// <summary>
        /// 支付状态消息
        /// </summary>
        [Description("支付状态消息")]
        [Column(StringLength = -1)]
        public string PayStatusMsg { get; set; }

        /// <summary>
        /// 是否回写
        /// </summary>
        [Description("是否回写")]
        public CallBack IsCallBack { get; set; }

        /// <summary>
        /// 是否担保交易(资金将冻结,直到发指令解冻)
        /// </summary>
        [Description("是否担保交易")]
        public bool IsSecuredTrade { get; set; }

        /// <summary>
        /// 担保交易收款用户Id
        /// </summary>
        [Description("担保交易收款用户Id")]
        public string SecuredTradeUserId { get; set; }

        /// <summary>
        /// 担保交易状态
        /// </summary>
        [Description("担保交易状态")]
        public SecuredTradeStatus SecuredTradeStatus { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        [Description("过期时间")]
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        [Column(StringLength = 100)]
        public string Remark { get; set; }

    }
}
