using LY.Report.Core.Model.Article.Enum;
using LY.Report.Core.Model.BaseEnum;

namespace LY.Report.Core.Service.Article.Type.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class ArticleTypeGetInput
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        public string ArticleTypeId { get; set; }

        /// <summary>
        /// 分类类型
        /// </summary>
        public ArticleCategory ArticleCategory { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string ArticleTypeName { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public IsActive IsActive { get; set; }
    }
}
