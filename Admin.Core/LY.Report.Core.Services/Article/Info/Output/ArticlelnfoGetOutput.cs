using System;
using LY.Report.Core.Model.Article.Enum;
using LY.Report.Core.Model.BaseEnum;

namespace LY.Report.Core.Service.Article.Info.Output
{
    /// <summary>
    /// 查询
    /// </summary>
    public class ArticlelnfoGetOutput
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        public string ArticleTypeId { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string ArticleTypeName { get; set; }

        /// <summary>
        /// 分类类型
        /// </summary>
        public ArticleCategory ArticleCategory { get; set; }

        /// <summary>
        /// 文章状态
        /// </summary>
        public ArticleStatus ArticleStatus { get; set; }

        /// <summary>
        /// 文章设置
        /// </summary>
        public ArticleSetting ArticleSetting { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容摘要
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string ArticleContent { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime PublishDate { get; set; }

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
