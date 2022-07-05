using System;

namespace LY.Report.Core.LYApiUtil.Pay.In
{
    /// <summary>
    /// 查询
    /// </summary>
    public class RechargeRecordGetIn 
    {
        /// <summary>
        /// 用户Id
        /// 限制为36个字符,必填
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 商户充值单号
        /// 限制为2-64个字符,选填
        /// </summary>
        public string RechargeOutTradeNo { get; set; }

        /// <summary>
        /// 交易开始时间(格式2021-01-01)(格式2021-01-01)
        /// 下单结束时间+下单结束时间需同时填写,否则无效
        /// 选填
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 交易结束时间(格式2021-01-01)
        /// 下单结束时间+下单结束时间需同时填写,否则无效
        /// 选填
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
