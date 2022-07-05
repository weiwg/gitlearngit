using System;
using System.ComponentModel;
using LY.Report.Core.Model.Pay.Enum;
using LY.Report.Core.Service.Pay.Refund.Input;

namespace LY.Report.Core.Service.Pay.Refund.Output
{
    public class PayRefundGetOutput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 商户退款单号
        /// </summary>
        public string RefundOutTradeNo { get; set; }

        /// <summary>
        /// 商户单号(原单号)
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 平台退款单号
        /// </summary>
        public string RefundTradeNo { get; set; }

        /// <summary>
        /// 平台单号(原单号)
        /// </summary>
        public string TradeNo { get; set; }

        /// <summary>
        /// 退款平台
        /// </summary>
        public FundPlatform FundPlatform { get; set; }

        /// <summary>
        /// 支付金额(原订单金额)
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 退款说明
        /// </summary>
        public string RefundDescription { get; set; }

        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RefundAmount { get; set; }

        /// <summary>
        /// 退款手续费
        /// </summary>
        public decimal RefundCharge { get; set; }

        /// <summary>
        /// 实际退款金额
        /// </summary>
        public decimal ActualRefundAmount { get; set; }

        /// <summary>
        /// 退款时间
        /// </summary>
        [Description("退款时间")]
        public DateTime? RefundDate { get; set; }

        /// <summary>
        /// 退款状态
        /// </summary>
        public RefundStatus RefundStatus { get; set; }

        /// <summary>
        /// 退款状态码
        /// </summary>
        public string RefundStatusCode { get; set; }

        /// <summary>
        /// 退款状态消息
        /// </summary>
        public string RefundStatusMsg { get; set; }

        /// <summary>
        /// 是否回写(0未回写  1 已回写)
        /// </summary>
        public CallBack IsCallBack { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
