
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Pay.Enum;
using LY.Report.Core.Util.Tool;
using FluentValidation;

namespace LY.Report.Core.Service.Pay.UaTrade.Input.Valid
{
    /// <summary>
    /// 验证规则
    /// </summary>
    public class PayOrderAddInputValidator : AbstractValidator<PayOrderAddInput>
    {
        public PayOrderAddInputValidator()
        {
            RuleFor(x => x.OutTradeNo).NotEmpty().WithName("商户单号");
            RuleFor(x => x.PayPlatform).Must(v=> EnumHelper.CheckEnum<FundPlatform>(v)).WithMessage("支付平台错误");
        }
    }
}
