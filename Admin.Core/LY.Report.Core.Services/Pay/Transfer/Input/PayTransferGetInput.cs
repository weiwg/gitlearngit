using System;
using LY.Report.Core.Model.Pay.Enum;

namespace LY.Report.Core.Service.Pay.Transfer.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class PayTransferGetInput
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
        /// 转账类型
        /// </summary>
        public TransferType TransferType { get; set; }

        /// <summary>
        /// 转账状态
        /// </summary>
        public TransferStatus TransferStatus { get; set; }

        /// <summary>
        /// 是否回写
        /// </summary>
        public CallBack IsCallBack { get; set; }

        /// <summary>
        /// 交易开始时间(格式2021-01-01)
        /// 交易开始时间+开始结束时间需同时填写,否则无效
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 交易结束时间(格式2021-01-01)
        /// 交易开始时间+开始结束时间需同时填写,否则无效
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
