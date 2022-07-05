using AutoMapper;
using LY.Report.Core.Model.Article;
using LY.Report.Core.Service.Article.Info.Input;
using LY.Report.Core.Service.Article.Info.Output;

namespace LY.Report.Core.Service.Article.Info
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<ArticleInfoAddInput, ArticleInfo>();
            CreateMap<ArticleInfoUpdateInput, ArticleInfo>();

            CreateMap<ArticleInfo, ArticlelnfoListOutput>();
            CreateMap<ArticleInfo, ArticlelnfoGetOutput>();
        }
    }
}
