using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.Common.Helpers;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Resource.Enum;
using LY.Report.Core.Service.Resource.ImgGallery;
using LY.Report.Core.Service.Resource.ImgGallery.Input;
using LY.Report.Core.Service.Resource.ImgGallery.Output;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Areas.Mobile.V1.Resource.Controllers
{
    /// <summary>
    /// 图片管理
    /// </summary>
    public class ImageController : BaseAreaController
    {
        private readonly IUser _user;
        private readonly IResImgGalleryService _resImgGalleryService;
        private readonly UploadConfig _uploadConfig;
        private readonly UploadHelper _uploadHelper;

        public ImageController(
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
        /// 上传图片
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
            var checkRes = await _resImgGalleryService.GetOneAsync(new ResImgGalleryGetInput { ImgMd5 = imgMd5, UserId = _user.UserId });
            if (checkRes.Success)
            {
                if (checkRes is ResponseOutput<ResImgGalleryGetOutput> output && output.Data != null && output.Data.ImgId.IsNotNull())
                {
                    //存在则直接返回
                    return ResponseOutput.Data(new { url = output.Data.FilePath, eurl = EncryptHelper.Aes.Encrypt(output.Data.FilePath) });
                    //return ResponseOutput.Data(output.Data.FilePath);
                }
            }
            var res = await _uploadHelper.UploadAsync(input.File, config, new { _user.UserId });
            if (!res.Success)
            {
                return ResponseOutput.NotOk(res.Msg);
            }

            var fileInfo = res.Data;
            //保存文档图片
            var addRes = await _resImgGalleryService.AddAsync(
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
            if (addRes.Success)
            {
                //返回路径+加密路径(后台使用前需解密,以校验数据准确性)
                return ResponseOutput.Data(new { url = res.Data.FileRequestPath, eurl = EncryptHelper.Aes.Encrypt(res.Data.FileRequestPath) });
                //return ResponseOutput.Data(res.Data.FileRequestPath);
            }

            return ResponseOutput.NotOk("上传失败！");
        }
    }


}
