namespace LY.Report.Core.Service.Product.Abnormals.Input
{
    public class ProductAbnormalUpdateInput:ProductAbnormalAddInput
    {
        /// <summary>
        /// 异常单号
        /// </summary>
        public string AbnormalNo { get; set; }
    }
}
