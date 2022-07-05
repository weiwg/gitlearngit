
using FluentValidation;

namespace LY.Report.Core.Service.Delivery.CarType.Input.Valid
{
    /// <summary>
    /// 验证规则
    /// </summary>
    public class DeliveryCarTypeUpdateInputValidator : AbstractValidator<DeliveryCarTypeUpdateInput>
    {
        public DeliveryCarTypeUpdateInputValidator()
        {
            Include(new DeliveryCarTypeAddInputValidator());
            RuleFor(x => x.CarId).NotEmpty().WithName("车型Id");
        }
    }
}
