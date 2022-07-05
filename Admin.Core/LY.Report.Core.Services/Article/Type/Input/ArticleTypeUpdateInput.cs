
namespace LY.Report.Core.Service.Article.Type.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public class ArticleTypeUpdateInput : ArticleTypeAddInput
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        public string ArticleTypeId { get; set; }
    }
}
