using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Resource.ImgData.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class ResImgDataAddInput
    {
        /// <summary>
        /// 图片Md5
        /// </summary>
        [Required(ErrorMessage = "请选择图片！")]
        public string ImgMd5 { get; set; }

        /// <summary>
        /// 图片数据(Base64)
        /// </summary>
        [Required(ErrorMessage = "图片数据不存在！")]
        public string ImgData { get; set; }
    }
}
