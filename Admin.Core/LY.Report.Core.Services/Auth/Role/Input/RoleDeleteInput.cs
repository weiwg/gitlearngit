using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Auth.Role.Input
{
    public class RoleDeleteInput
    {
        [Required(ErrorMessage = "角色Id不能为空")]
        public string Id { get; set; }
    }
}
