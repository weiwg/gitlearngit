
using FluentValidation;

namespace LY.Report.Core.Service.Article.Info.Input.Valid
{
    /// <summary>
    /// 验证规则
    /// </summary>
    public class ArticleInfoUpdateInputValidator : AbstractValidator<ArticleInfoUpdateInput>
    {
        public ArticleInfoUpdateInputValidator()
        {
            Include(new ArticleInfoAddInputValidator());
            RuleFor(x => x.ArticleId).NotEmpty().WithName("文章Id");
        }
    }
}
