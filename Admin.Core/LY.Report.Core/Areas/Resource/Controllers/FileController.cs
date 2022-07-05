using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Auth;
using EonUp.Delivery.Core.Common.Configs;
using EonUp.Delivery.Core.Common.Helpers;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Model.Resource.Enum;
using EonUp.Delivery.Core.Service.Resource.ImgGallery;
using EonUp.Delivery.Core.Service.Resource.ImgGallery.Input;
using EonUp.Delivery.Core.Util.Common;
using EonUp.Delivery.Core.Util.Tool;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EonUp.Delivery.Core.Areas.Resource.Controllers
{
    /// <summary>
    /// 文件管理
    /// </summary>
    public class FileController : BaseAreaController
    {
        private readonly IUser _user;
        private readonly IResImgGalleryService _resImgGalleryService;
        private readonly UploadConfig _uploadConfig;
        private readonly UploadHelper _uploadHelper;

        public FileController(
            IUser user,
            IResImgGalleryService resImgGalleryService,
            IOptionsMonitor<UploadConfig> uploadConfig,
            UploadHelper uploadHelper)
        {
            _user = user;
            _resImgGalleryService = resImgGalleryService;
            _uploadConfig = uploadConfig.CurrentValue;
            _uploadHelper = uploadHelper;
        }

        /// <summary>
        /// 上传文档图片
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> UploadImage([FromForm] ResFileUploadInput input)
        {
            FileUploadConfig config;
            switch (input.FileCategory)
            {
                case FileCategory.Avatar:
                    config = _uploadConfig.Avatar;
                    break;
                case FileCategory.Document:
                    config = _uploadConfig.Document;
                    break;
                case FileCategory.Certificate:
                    config = _uploadConfig.Certificate;
                    break;
                default:
                    return ResponseOutput.NotOk("上传类型错误！");
            }

            string imgMd5 = ImageHelper.CreatFileSha1(input.File.OpenReadStream());
            //此处检查文件是否存在
            var res = await _uploadHelper.UploadAsync(input.File, config, new { _user.UserId });
            if (res.Success)
            {
                var fileInfo = res.Data;
                //保存文档图片
                var r = await _resImgGalleryService.AddAsync(
                    new ResImgGalleryAddInput
                    {
                        ImgMd5 = imgMd5,
                        ImgCategory = input.FileCategory,
                        FileName = fileInfo.SaveName,
                        FileExtension = fileInfo.Extension,
                        FileSize = CommonHelper.GetInt(fileInfo.Size.GetSize()),
                        FilePath = res.Data.FileRequestPath,
                        UserId = _user.UserId
                    });
                if (r.Success)
                {
                    return ResponseOutput.Ok(res.Data.FileRequestPath);
                }
            }

            return ResponseOutput.NotOk("上传失败！");
        }
    }
}
