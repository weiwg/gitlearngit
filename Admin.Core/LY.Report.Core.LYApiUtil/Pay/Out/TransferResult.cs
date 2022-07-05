using System;
using LY.Report.Core.LYApiUtil.Pay.Enum;

namespace LY.Report.Core.LYApiUtil.Pay.Out
{
    /// <summary>
    /// 转账状态
    /// </summary>
    public class TransferResult
    {
        /// <summary>
        /// 商户转账单号
        /// </summary>
        public string TransferOutTradeNo { get; set; }

        /// <summary>
        /// 平台转账单号
        /// </summary>
        public string TransferTradeNo { get; set; }

        /// <summary>
        /// 转账平台
        /// </summary>
        public FundPlatform FundPlatform { get; set; }

        /// <summary>
        /// 转账平台描述
        /// </summary>
        public string FundPlatformDescribe { get; set; }

        /// <summary>
        /// 转账说明
        /// </summary>
        public string TransferDescription { get; set; }

        /// <summary>
        /// 转账金额
        /// </summary>
        public decimal TransferAmount { get; set; }

        /// <summary>
        /// 转账手续费
        /// </summary>
        public decimal TransferCharge { get; set; }

        /// <summary>
        /// 实际转账金额
        /// </summary>
        public decimal ActualTransferAmount { get; set; }

        /// <summary>
        /// 转账时间
        /// </summary>
        public DateTime TransferDate { get; set; }

        /// <summary>
        /// 到账时间
        /// </summary>
        public DateTime? TransferFinishDate { get; set; }

        /// <summary>
        /// 转账状态
        /// </summary>
        public TransferStatus TransferStatus { get; set; }

        /// <summary>
        /// 转账状态描述
        /// </summary>
        public string TransferStatusDescribe { get; set; }

        /// <summary>
        /// 转账状态码
        /// </summary>
        public string TransferStatusCode { get; set; }

        /// <summary>
        /// 转账状态消息
        /// </summary>
        public string TransferStatusMsg { get; set; }
    }
}
