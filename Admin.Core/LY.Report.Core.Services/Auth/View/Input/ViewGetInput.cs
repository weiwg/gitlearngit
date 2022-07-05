namespace LY.Report.Core.Service.Auth.View.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class ViewGetInput
    {
        /// <summary>
        /// 视图名/地址
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string ApiVersion { get; set; }
    }
}
