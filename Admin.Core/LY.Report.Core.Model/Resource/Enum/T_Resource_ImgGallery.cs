using System;
using System.ComponentModel;

namespace LY.Report.Core.Model.Resource.Enum
{
    /// <summary>
    /// 图片分类
    /// </summary>
    public enum FileCategory
    {
        /// <summary>
        /// 头像
        /// </summary>
        [Description("头像")]
        Avatar = 100,
        /// <summary>
        /// 文档
        /// </summary>
        [Description("文档")]
        Document = 200,
        /// <summary>
        /// 证件
        /// </summary>
        [Description("证件")]
        Certificate = 300,
        /// <summary>
        /// 聊天
        /// </summary>
        [Description("聊天")]
        Chat = 400
    }

    /// <summary>
    /// 保存类型
    /// </summary>
    [Serializable]
    public enum ImgSaveType
    {
        /// <summary>
        /// 文件
        /// </summary>
        [Description("文件")]
        File = 1,
        /// <summary>sss
        /// 数据库(Base64)
        /// </summary>
        [Description("数据库")]
        Database = 2
    }
}
