namespace LY.Report.Core.Service.Pay.Transfer.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class PayTransferUpdateInput : PayTransferAddInput
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
