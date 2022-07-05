using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Resource.Enum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Resource
{
    /// <summary>
    /// 图库
    /// </summary>
    [Table(Name = "T_Resource_ImgGallery")]
    [Index("idx_{tablename}_01", nameof(ImgId), true)]
    public class ResImgGallery : EntityTenantFull
    {
        /// <summary>
        /// 图片Id
        /// </summary>
        [Description("图片Id")]
        [Column(IsPrimary = true, IsNullable = false, StringLength = 36)]
        public string ImgId { get; set; }

        /// <summary>
        /// 图片Md5
        /// </summary>
        [Description("图片Md5")]
        [Column(IsNullable = false, StringLength = 50)]
        public string ImgMd5 { get; set; }

        /// <summary>
        /// 图片分类
        /// </summary>
        [Description("图片分类")]
        [Column(IsNullable = false)]
        public FileCategory ImgCategory { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        [Description("文件名")]
        [Column(IsNullable = false, StringLength = 100)]
        public string FileName { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        [Description("文件扩展名")]
        [Column(IsNullable = false, StringLength = 100)]
        public string FileExtension { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        [Description("文件大小")]
        [Column(IsNullable = false)]
        public int FileSize { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [Description("文件路径")]
        [Column(IsNullable = false, StringLength = 500)]
        public string FilePath { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Description("用户Id")]
        [Column(IsNullable = false, StringLength = 36)]
        public string UserId { get; set; }
    }
}