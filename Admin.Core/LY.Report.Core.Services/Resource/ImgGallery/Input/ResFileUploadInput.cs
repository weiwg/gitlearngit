using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.Resource.Enum;

namespace LY.Report.Core.Service.Resource.ImgGallery.Input
{
    public class ResFileUploadInput
    {
        /// <summary>
        /// �ϴ��ļ�
        /// </summary>
        [Required(ErrorMessage = "��ѡ���ϴ����ļ���")]
        public IFormFile File { get; set; }

        /// <summary>
        /// ͼƬ����
        /// </summary>
        [Required(ErrorMessage = "�ϴ����Ͳ���Ϊ�գ�")]
        public FileCategory FileCategory { get; set; }
    }
}
