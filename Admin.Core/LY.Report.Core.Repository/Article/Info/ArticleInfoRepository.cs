using LY.Report.Core.Common.Auth;
using LY.Report.Core.Model.Article;

namespace LY.Report.Core.Repository.Article.Info
{
    public class ArticleInfoRepository : RepositoryBase<ArticleInfo>, IArticleInfoRepository
    {
        public ArticleInfoRepository(MyUnitOfWorkManager muowm, IUser user) : base(muowm, user)
        {

        }
    }
}
