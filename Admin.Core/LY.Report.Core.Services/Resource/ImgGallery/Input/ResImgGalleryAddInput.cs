using LY.Report.Core.Model.Resource.Enum;

namespace LY.Report.Core.Service.Resource.ImgGallery.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class ResImgGalleryAddInput
    {
        /// <summary>
        /// 图片Md5
        /// </summary>
        public string ImgMd5 { get; set; }

        /// <summary>
        /// 图片分类
        /// </summary>
        public FileCategory ImgCategory { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }
    }
}
