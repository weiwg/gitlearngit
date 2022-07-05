namespace LY.Report.Core.Service.Auth.Role.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class RoleUpdateInput :RoleAddInput
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 更新用户Id
        /// </summary>
        public string UpdateUserId { get; set; }

    }
}
