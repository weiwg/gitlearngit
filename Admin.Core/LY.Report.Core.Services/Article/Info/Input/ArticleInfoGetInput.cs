using LY.Report.Core.Model.Article.Enum;
using LY.Report.Core.Model.BaseEnum;

namespace LY.Report.Core.Service.Article.Info.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class ArticleInfoGetInput
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public string ArticleId { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        public string ArticleTypeId { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 分类类型
        /// </summary>
        public ArticleCategory ArticleCategory { get; set; }

        /// <summary>
        /// 文章分类名称
        /// </summary>
        public string ArticleTypeName { get; set; }

        /// <summary>
        /// 文章状态
        /// </summary>
        public ArticleStatus ArticleStatus { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public IsActive IsActive { get; set; }

    }
}
