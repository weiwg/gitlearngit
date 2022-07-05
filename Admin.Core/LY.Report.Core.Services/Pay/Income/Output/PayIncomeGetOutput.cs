using System;
using LY.Report.Core.Model.Pay.Enum;

namespace LY.Report.Core.Service.Pay.Income.Output
{
    public class PayIncomeGetOutput
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
        /// 支付订单类型
        /// </summary>
        public PayOrderType PayOrderType { get; set; }

        /// <summary>
        /// 支付描述
        /// </summary>
        public string PayDescription { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// 系统补贴金额
        /// 系统优惠券,系统红包,积分抵扣等,系统补贴给收款用户
        /// </summary>
        public decimal AppSubsidyAmount { get; set; }

        /// <summary>
        /// App交易手续费(包括平台手续费)
        /// </summary>
        public decimal PayAppCharge { get; set; }

        /// <summary>
        /// 平台交易手续费, 用于计算支付平台扣除的手续费
        /// </summary>
        public decimal PayPlatformCharge { get; set; }

        /// <summary>
        /// 实际支付金额(支付平台返回)
        /// </summary>
        public decimal ActualPayAmount { get; set; }

        /// <summary>
        /// 实际结算交易手续费
        /// </summary>
        public decimal ActualSettlePayCharge { get; set; }

        /// <summary>
        /// 实际结算金额(ActualPayAmount-ActualSettlePayCharge)
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
        /// 支付状态码
        /// </summary>
        public string PayStatusCode { get; set; }

        /// <summary>
        /// 支付状态消息
        /// </summary>
        public string PayStatusMsg { get; set; }

        /// <summary>
        /// 是否回写
        /// </summary>
        public CallBack IsCallBack { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
