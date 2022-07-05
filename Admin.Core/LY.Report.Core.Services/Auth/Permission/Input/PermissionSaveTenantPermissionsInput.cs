using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Auth.Permission.Input
{
    public class PermissionSaveTenantPermissionsInput
    {
        [Required(ErrorMessage = "租户不能为空！")]
        public string TenantId { get; set; }

        [Required(ErrorMessage = "权限不能为空！")]
        public List<string> PermissionIds { get; set; }
    }
}
