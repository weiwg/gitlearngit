
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Util.Tool;
using FluentValidation;

namespace LY.Report.Core.Service.Demo.Test.Input.Valid
{
    /// <summary>
    /// 验证规则
    /// </summary>
    public class DemoTestAddInputValidator : AbstractValidator<DemoTestAddInput>
    {
        public DemoTestAddInputValidator()
        {
            RuleSet("Test", () => {
                RuleFor(x => x.TestName).NotEmpty().WithName("名称");
            }); 

            RuleFor(x => x.TestName).NotEmpty().MinimumLength(2).MaximumLength(20).WithName("名称");
            RuleFor(x => x.TestImg).NotEmpty().WithName("图片");
            RuleFor(x => x.TestCount).NotEmpty().GreaterThan(0).WithName("数量");
            RuleFor(x => x.TestPrice).NotEmpty().GreaterThanOrEqualTo(0.01m).WithName("价格");
            RuleFor(x => x.IsActive).Must(v=> EnumHelper.CheckEnum<IsActive>(v)).WithMessage("有效性错误");
            RuleFor(x => x.Remark).MaximumLength(10).WithName("备注").WithMessage("'{PropertyName}'必须小于或等于{MaxLength}个字符。");
        }
    }
}
