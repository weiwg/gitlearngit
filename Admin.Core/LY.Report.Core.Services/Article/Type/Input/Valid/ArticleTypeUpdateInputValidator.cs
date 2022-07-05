
using FluentValidation;

namespace LY.Report.Core.Service.Article.Type.Input.Valid
{
    /// <summary>
    /// 验证规则
    /// </summary>
    public class ArticleTypeUpdateInputValidator : AbstractValidator<ArticleTypeUpdateInput>
    {
        public ArticleTypeUpdateInputValidator()
        {
            Include(new ArticleTypeAddInputValidator());
            RuleFor(x => x.ArticleTypeId).NotEmpty().WithName("分类Id");
        }
    }
}
