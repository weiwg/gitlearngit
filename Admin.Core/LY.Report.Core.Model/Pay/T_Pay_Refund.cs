using System;
using System.ComponentModel;
using FreeSql.DataAnnotations;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Pay.Enum;

namespace LY.Report.Core.Model.Pay
{
    /// <summary>
    /// 退款记录表
    /// </summary>
    [Table(Name = "T_Pay_Refund")]
    [Index("idx_{tablename}_01", "AppId AES, RefundOutTradeNo AES", true)]
    public class PayRefund : EntityTenantFull
    {
        /// <summary>
        /// 退款记录Id
        /// </summary>
        [Description("退款记录Id")]
        [Column(Position = 2, StringLength = 36, IsNullable = false)]
        public string RefundId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Description("用户Id")]
        [Column(StringLength = 36, IsNullable = false)]
        public string UserId { get; set; }

        /// <summary>
        /// 商户退款单号
        /// </summary>
        [Description("商户退款单号")]
        [Column(IsPrimary = true, StringLength = 64, IsNullable = false)]
        public string RefundOutTradeNo { get; set; }

        /// <summary>
        /// 商户单号(原单号)
        /// </summary>
        [Description("商户单号")]
        [Column(StringLength = 64, IsNullable = false)]
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 平台退款单号
        /// </summary>
        [Description("平台退款单号")]
        [Column(StringLength = 64, IsNullable = false)]
        public string RefundTradeNo { get; set; }

        /// <summary>
        /// 平台单号(原单号)
        /// </summary>
        [Description("平台单号")]
        [Column(StringLength = 64, IsNullable = false)]
        public string TradeNo { get; set; }

        /// <summary>
        /// 退款平台
        /// </summary>
        [Description("退款平台")]
        public FundPlatform FundPlatform { get; set; }

        /// <summary>
        /// 支付金额(原订单金额)
        /// </summary>
        [Description("支付金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 系统补贴金额(原订单金额)
        /// </summary>
        [Description("系统补贴金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal AppSubsidyAmount { get; set; }

        /// <summary>
        /// 退款说明
        /// </summary>
        [Description("退款说明")]
        [Column(StringLength = 100, IsNullable = false)]
        public string RefundDescription { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        [Description("退款金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal RefundAmount { get; set; }

        /// <summary>
        /// 退款系统补贴金额
        /// </summary>
        [Description("退款系统补贴金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal RefundAppSubsidyAmount { get; set; }

        /// <summary>
        /// 退款手续费
        /// </summary>
        [Description("退款手续费")]
        [Column(Precision = 12, Scale = 2)]
        public decimal RefundCharge { get; set; }

        /// <summary>
        /// 实际退款金额
        /// </summary>
        [Description("实际退款金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal ActualRefundAmount { get; set; }

        /// <summary>
        /// 退款时间
        /// </summary>
        [Description("退款时间")]
        public DateTime? RefundDate { get; set; }

        /// <summary>
        /// 退款状态
        /// </summary>
        [Description("退款状态")]
        public RefundStatus RefundStatus { get; set; }

        /// <summary>
        /// 退款状态码
        /// </summary>
        [Description("退款状态码")]
        [Column(StringLength = -1)]
        public string RefundStatusCode { get; set; }

        /// <summary>
        /// 退款状态消息
        /// </summary>
        [Description("退款状态消息")]
        [Column(StringLength = -1)]
        public string RefundStatusMsg { get; set; }

        /// <summary>
        /// 是否回写(0未回写  1 已回写)
        /// </summary>
        [Description("是否回写")]
        public CallBack IsCallBack { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        [Column(StringLength = 100)]
        public string Remark { get; set; }

    }
}
