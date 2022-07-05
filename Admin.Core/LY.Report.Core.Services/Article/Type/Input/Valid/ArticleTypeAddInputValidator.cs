using LY.Report.Core.Model.Article.Enum;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Util.Tool;
using FluentValidation;

namespace LY.Report.Core.Service.Article.Type.Input.Valid
{
    /// <summary>
    /// 验证规则
    /// </summary>
    public class ArticleTypeAddInputValidator : AbstractValidator<ArticleTypeAddInput>
    {
        public ArticleTypeAddInputValidator()
        {
            RuleFor(x => x.ArticleCategory).Must(v => EnumHelper.CheckEnum<ArticleCategory>(v)).WithMessage("分类类型错误");
            RuleFor(x => x.ArticleTypeName).NotEmpty().WithName("分类名称");
            RuleFor(x => x.ParentId).NotEmpty().WithName("父级Id");
            RuleFor(x => x.Sequence).NotEmpty().GreaterThanOrEqualTo(0).WithName("排序");
            RuleFor(x => x.IsActive).Must(v => EnumHelper.CheckEnum<IsActive>(v)).WithMessage("有效性错误");
        }
    }
}
