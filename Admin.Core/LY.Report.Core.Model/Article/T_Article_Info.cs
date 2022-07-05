using System;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Article.Enum;
using LY.Report.Core.Model.BaseEnum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Article
{
    /// <summary>
    /// 文章
    /// </summary>
    [Table(Name = "T_Article_Info")]
    [Index("idx_{tablename}_01", nameof(ArticleId), true)]
    public class ArticleInfo: EntityTenantFull
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        [Description("文章Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string ArticleId { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        [Description("分类Id")]
        [Column(StringLength = 36)]
        public string ArticleTypeId { get; set; }

        /// <summary>
        /// 文章状态
        /// </summary>
        [Description("文章状态")]
        public ArticleStatus ArticleStatus { get; set; }

        /// <summary>
        /// 文章设置
        /// </summary>
        [Description("文章设置")]
        public ArticleSetting ArticleSetting { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Description("标题")]
        [Column(StringLength = 50, IsNullable = false)]
        public string Title { get; set; }

        /// <summary>
        /// 内容摘要
        /// </summary>
        [Description("内容摘要")]
        [Column(StringLength = 200, IsNullable = false)]
        public string Abstract { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Description("内容")]
        [Column(StringLength = -1, IsNullable = false)]
        public string ArticleContent { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        [Description("发布日期")]
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 排序(越大越靠前)
        /// </summary>
        [Description("排序")]
        public int Sequence { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        [Description("是否有效")]
        [Column(IsNullable = false)]
        public IsActive IsActive { get; set; }
    }
}
