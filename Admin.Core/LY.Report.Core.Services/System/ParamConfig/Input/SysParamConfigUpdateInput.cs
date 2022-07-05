namespace LY.Report.Core.Service.System.ParamConfig.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class SysParamConfigUpdateInput : SysParamConfigAddInput
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
