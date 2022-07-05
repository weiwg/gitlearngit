namespace LY.Report.Core.Service.Auth.Api.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class ApiUpdateInput :ApiAddInput
    {
        /// <summary>
        /// 接口Id
        /// </summary>
        public string ApiId { get; set; }

        /// <summary>
        /// 更新者用户ID
        /// </summary>
        public string UpdateUserId { get; set; }
    }
}
