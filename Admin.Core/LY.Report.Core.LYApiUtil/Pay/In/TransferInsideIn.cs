using LY.Report.Core.LYApiUtil.Pay.Enum;

namespace LY.Report.Core.LYApiUtil.Pay.In
{
    /// <summary>
    /// 交易转账
    /// </summary>
    public class TransferInsideIn
    {
        /// <summary>
        /// 用户Id
        /// 限制为36个字符,必填
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 来源用户Id
        /// TransferInsideType.User时不能为空
        /// 限制为36个字符,选填
        /// </summary>
        public string FromUserId { get; set; }

        /// <summary>
        /// 商户转账单号
        /// 限制为2-64个字符,必填
        /// </summary>
        public string TransferOutTradeNo { get; set; }

        /// <summary>
        /// 转账类型
        /// 必填
        /// </summary>
        public TransferInsideType TransferInsideType { get; set; }

        /// <summary>
        /// 转账说明
        /// 限制为2-100个字符,必填
        /// </summary>
        public string TransferDescription { get; set; }

        /// <summary>
        /// 转账金额
        /// 单位元,保留2位小数,大于0,必填
        /// </summary>
        public decimal TransferAmount { get; set; }
    }
}
