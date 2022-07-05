namespace LY.Report.Core.Service.Auth.Permission.Input
{
    public class PermissionUpdateApiInput : PermissionAddApiInput
    {
        /// <summary>
        /// 权限Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 权限Id（主键）
        /// </summary>
        public string PermissionId { get; set; }
    }
}
