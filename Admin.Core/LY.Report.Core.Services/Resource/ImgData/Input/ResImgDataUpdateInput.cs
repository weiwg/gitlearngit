namespace LY.Report.Core.Service.Resource.ImgData.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class ResImgDataUpdateInput : ResImgDataAddInput
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
