using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Article;

namespace LY.Report.Core.Repository.Article.Type
{
    public class ArticleTypeRepository : RepositoryBase<ArticleType>, IArticleTypeRepository
    {
        public ArticleTypeRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {

        }
    }
}
