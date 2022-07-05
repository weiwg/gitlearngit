
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Util.Tool;
using FluentValidation;

namespace LY.Report.Core.Service.Delivery.CarType.Input.Valid
{
    /// <summary>
    /// 验证规则
    /// </summary>
    public class DeliveryCarTypeAddInputValidator : AbstractValidator<DeliveryCarTypeAddInput>
    {
        public DeliveryCarTypeAddInputValidator()
        {
            RuleSet("Test", () => {
                RuleFor(x => x.CarName).NotEmpty().WithName("车型名称");
            }); 

            RuleFor(x => x.CarName).NotEmpty().MinimumLength(2).MaximumLength(20).WithName("车型名称");
            RuleFor(x => x.CarImg).NotEmpty().WithName("车型图片");
            RuleFor(x => x.LoadWeight).NotEmpty().GreaterThan(0).WithName("装载重量");
            RuleFor(x => x.LoadVolume).NotEmpty().GreaterThan(0).WithName("装载体积");
            RuleFor(x => x.LoadSize).NotEmpty().MaximumLength(50).WithName("装载尺寸");
            RuleFor(x => x.IsActive).Must(v=> EnumHelper.CheckEnum<IsActive>(v)).WithName("是否有效");
            RuleFor(x => x.Remark).MaximumLength(10).WithName("备注").WithMessage("'{PropertyName}'必须小于或等于{MaxLength}个字符。");
        }
    }
}
