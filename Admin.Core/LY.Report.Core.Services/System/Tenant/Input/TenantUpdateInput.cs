namespace LY.Report.Core.Service.System.Tenant.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class TenantUpdateInput : TenantAddInput
    {
        /// <summary>
        /// 接口Id
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// 更新者id
        /// </summary>
        public string UpdateUserId { get; set; }
    }
}
