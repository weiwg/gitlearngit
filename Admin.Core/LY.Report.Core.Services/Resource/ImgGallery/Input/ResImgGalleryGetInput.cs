using LY.Report.Core.Model.Resource;
using LY.Report.Core.Model.Resource.Enum;

namespace LY.Report.Core.Service.Resource.ImgGallery.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class ResImgGalleryGetInput
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
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
    }
}
