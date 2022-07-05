using System;

namespace LY.Report.Core.LYApiUtil.Pay.In
{
    /// <summary>
    /// 查询
    /// </summary>
    public class TransferQueryIn
    {
        /// <summary>
        /// 用户Id
        /// 选填
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 商户转账单号
        /// 选填
        /// </summary>
        public string TransferOutTradeNo { get; set; }

        /// <summary>
        /// 平台转账单号
        /// 选填
        /// </summary>
        public string TransferTradeNo { get; set; }

        /// <summary>
        /// 转账开始时间(格式2021-01-01)
        /// 下单结束时间+下单结束时间需同时填写,否则无效
        /// 选填
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 转账结束时间
        /// 下单结束时间+下单结束时间需同时填写,否则无效
        /// 选填
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
