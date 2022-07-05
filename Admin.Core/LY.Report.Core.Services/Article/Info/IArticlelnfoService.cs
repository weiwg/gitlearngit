using LY.Report.Core.Service.Article.Info.Input;
using LY.Report.Core.Service.Base.IService;

namespace LY.Report.Core.Service.Article.Info
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IArticleInfoService : IBaseService, IAddService<ArticleInfoAddInput>, IUpdateEntityService<ArticleInfoUpdateInput>, IGetService<ArticleInfoGetInput>, ISoftDeleteBaseService
    {
    }
}