using LY.Report.Core.Model.Article.Enum;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Util.Tool;
using FluentValidation;

namespace LY.Report.Core.Service.Article.Info.Input.Valid
{
    /// <summary>
    /// 验证规则
    /// </summary>
    public class ArticleInfoAddInputValidator : AbstractValidator<ArticleInfoAddInput>
    {
        public ArticleInfoAddInputValidator()
        {
            RuleFor(x => x.ArticleTypeId).NotEmpty().WithName("分类Id");
            RuleFor(x => x.ArticleStatus).Must(v => EnumHelper.CheckEnum<ArticleStatus>(v)).WithMessage("文章状态错误");
            RuleFor(x => x.ArticleSetting).Must(v => EnumHelper.CheckEnum<ArticleSetting>(v)).WithMessage("文章设置错误");
            RuleFor(x => x.Title).NotEmpty().MinimumLength(2).MaximumLength(30).WithName("标题");
            RuleFor(x => x.Abstract).NotEmpty().MinimumLength(2).MaximumLength(100).WithName("摘要");
            RuleFor(x => x.ArticleContent).NotEmpty().MinimumLength(2).WithName("内容");
            RuleFor(x => x.ArticleContent).Must(v => EncryptHelper.Base64.Decrypt(v).IsNotNull()).WithMessage("内容格式错误");
            RuleFor(x => x.Sequence).NotEmpty().GreaterThanOrEqualTo(0).WithName("排序");
            RuleFor(x => x.IsActive).Must(v => EnumHelper.CheckEnum<IsActive>(v)).WithMessage("有效性错误");
        }
    }
}
