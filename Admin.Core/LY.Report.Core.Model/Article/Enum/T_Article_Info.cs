using System.ComponentModel;

namespace LY.Report.Core.Model.Article.Enum
{
    /// <summary>
    /// 文章状态
    /// </summary>
    public enum ArticleStatus
    {
        /// <summary>
        /// 未发布
        /// </summary>
        [Description("未发布")]
        Unpublished = 1,
        /// <summary>
        /// 已发布
        /// </summary>
        [Description("已发布")]
        Published = 2,
    }

    /// <summary>
    /// 文章设置
    /// </summary>
    public enum ArticleSetting
    {
        /// <summary>
        /// 普通
        /// </summary>
        [Description("普通")]
        Normal = 1,
        /// <summary>
        /// 置顶
        /// </summary>
        [Description("置顶")]
        Top = 2,
        /// <summary>
        /// 推荐
        /// </summary>
        [Description("推荐")]
        Recommend = 3,
        /// <summary>
        /// 热门
        /// </summary>
        [Description("热门")]
        Hot = 4
    }
}
