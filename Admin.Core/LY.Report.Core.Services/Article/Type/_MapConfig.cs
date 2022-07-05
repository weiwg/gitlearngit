using AutoMapper;
using LY.Report.Core.Model.Article;
using LY.Report.Core.Service.Article.Type.Input;
using LY.Report.Core.Service.Article.Type.Output;

namespace LY.Report.Core.Service.Article.Type
{
    /// <summary>
    /// 映射配置
    /// </summary>
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<ArticleTypeAddInput, ArticleType>();
            CreateMap<ArticleTypeUpdateInput, ArticleType>();

            CreateMap<ArticleType, ArticleTypeListOutput>();
        }
    }
}
