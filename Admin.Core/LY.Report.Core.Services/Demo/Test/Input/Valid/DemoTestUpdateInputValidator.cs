
using FluentValidation;

namespace LY.Report.Core.Service.Demo.Test.Input.Valid
{
    /// <summary>
    /// 验证规则
    /// </summary>
    public class DeliveryCarTypeUpdateInputValidator : AbstractValidator<DemoTestUpdateInput>
    {
        public DeliveryCarTypeUpdateInputValidator()
        {
            Include(new DemoTestAddInputValidator());
            RuleFor(x => x.TestId).NotEmpty().WithName("TestId");
        }
    }
}
