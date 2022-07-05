using LY.Report.Core.LYApiUtil.Pay.Enum;

namespace LY.Report.Core.LYApiUtil.Pay.Out
{
    public class RechargeRecordGetOut
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        public string RecordId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 充值状态
        /// </summary>
        public RechargeStatus RechargeStatus { get; set; }

        /// <summary>
        /// 充值状态描述
        /// </summary>
        public string RechargeStatusDescribe { get; set; }

        /// <summary>
        /// 充值单号
        /// </summary>
        public string RechargeOrderNo { get; set; }

        /// <summary>
        /// 商户充值单号
        /// </summary>
        public string RechargeOutTradeNo { get; set; }

        /// <summary>
        /// 充值金额
        /// </summary>
        public decimal RechargeAmount { get; set; }

        /// <summary>
        /// 赠送金额
        /// </summary>
        public decimal GiveAmount { get; set; }

        /// <summary>
        /// 到账金额
        /// </summary>
        public decimal ActualArrivalAmount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
