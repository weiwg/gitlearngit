using LY.Report.Core.Service.Article.Type.Input;
using LY.Report.Core.Service.Base.IService;

namespace LY.Report.Core.Service.Article.Type
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IArticleTypeService : IBaseService, IAddService<ArticleTypeAddInput>, IUpdateEntityService<ArticleTypeUpdateInput>, IGetService<ArticleTypeGetInput>, ISoftDeleteBaseService
    {
    }
}