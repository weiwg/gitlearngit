using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Article.Enum;
using LY.Report.Core.Model.BaseEnum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Article
{
    /// <summary>
    /// 文章分类
    /// </summary>
    [Table(Name = "T_Article_Type")]
    [Index("idx_{tablename}_01", nameof(ArticleTypeId), true)]
   public class ArticleType: EntityTenantFull
    {
        /// <summary>
        /// 分类Id
        /// </summary>
        [Description("分类Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string ArticleTypeId { get; set; }

        /// <summary>
        /// 分类类型
        /// </summary>
        [Description("分类类型")]
        public ArticleCategory ArticleCategory { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [Description("分类名称")]
        [Column(StringLength = 50, IsNullable = false)]
        public string ArticleTypeName { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        [Description("父级Id")]
        [Column(StringLength = 36)]
        public string ParentId { get; set; }

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
