using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Auth.Permission.Input
{
    public class PermissionAssignInput
    {
        [Required(ErrorMessage = "角色不能为空！")]
        public string RoleId { get; set; }

        [Required(ErrorMessage = "权限不能为空！")]
        public List<string> PermissionIds { get; set; }
    }
}
