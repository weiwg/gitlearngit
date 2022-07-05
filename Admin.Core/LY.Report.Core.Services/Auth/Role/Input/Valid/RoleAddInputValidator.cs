using LY.Report.Core.Model.Auth.Enum;
using LY.Report.Core.Util.Tool;
using FluentValidation;

namespace LY.Report.Core.Service.Auth.Role.Input.Valid
{
    public class RoleAddInputValidator: AbstractValidator<RoleAddInput>
    {
        public RoleAddInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(20).WithName("角色名称");
            RuleFor(x => x.RoleType).Must(v => EnumHelper.CheckEnum<RoleType>(v)).WithName("编码").WithMessage("'{PropertyName}'有效性错误");
        }
    }
}
