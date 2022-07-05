using LY.Report.Core.Model.Article.Enum;
using LY.Report.Core.Model.BaseEnum;

namespace LY.Report.Core.Service.Article.Type.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class ArticleTypeAddInput
    {
        /// <summary>
        /// 分类类型
        /// </summary>
        public ArticleCategory ArticleCategory { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string ArticleTypeName { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 排序(越大越靠前)
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public IsActive IsActive { get; set; }
    }
}
