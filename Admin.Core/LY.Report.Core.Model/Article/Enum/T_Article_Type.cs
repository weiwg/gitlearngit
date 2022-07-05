
using System.ComponentModel;

namespace LY.Report.Core.Model.Article.Enum
{
    /// <summary>
    /// 分类类型
    /// </summary>
    public enum ArticleCategory
    {
        /// <summary>
        /// 文章
        /// </summary>
        [Description("文章")]
        Article = 1,
        /// <summary>
        /// 通知
        /// </summary>
        [Description("通知")]
        Notice = 2,
        /// <summary>
        /// 系统帮助
        /// </summary>
        [Description("系统帮助")]
        Help = 3
    }
}
