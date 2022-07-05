namespace LY.Report.Core.Service.Pay.Refund.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class PayRefundUpdateInput : PayRefundAddInput
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }
}
