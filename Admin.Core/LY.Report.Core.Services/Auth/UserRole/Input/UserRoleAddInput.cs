using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Auth.UserRole.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class UserRoleAddInput
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        [Required(ErrorMessage = "用户名称不能为空")]
        public string UserId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        [Display(Name = "角色ID")]
        [Required(ErrorMessage = "角色名称不能为空")]
        public string RoleId { get; set; }

        /// <summary>
        /// 创建用户角色人id
        /// </summary>
        [Display(Name = "创建用户ID")]
        public string CreateUserId { get; set; }
    }
}
