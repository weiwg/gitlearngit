using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.Resource.Enum;

namespace LY.Report.Core.Service.Resource.ImgGallery.Input
{
    public class ResFileUploadInput
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        [Required(ErrorMessage = "请选择上传的文件！")]
        public IFormFile File { get; set; }

        /// <summary>
        /// 图片分类
        /// </summary>
        [Required(ErrorMessage = "上传类型不能为空！")]
        public FileCategory FileCategory { get; set; }
    }
}
