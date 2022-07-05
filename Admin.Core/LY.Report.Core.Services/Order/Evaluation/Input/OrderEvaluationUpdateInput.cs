namespace LY.Report.Core.Service.Order.Evaluation.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class OrderEvaluationUpdateInput : OrderEvaluationAddInput
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
