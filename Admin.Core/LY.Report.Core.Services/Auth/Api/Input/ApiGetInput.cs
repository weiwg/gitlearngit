namespace LY.Report.Core.Service.Auth.Api.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class ApiGetInput
    {
        /// <summary>
        /// 接口或者地址名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Api版本号
        /// </summary>
        public string ApiVersion { get; set; }
    }
}
