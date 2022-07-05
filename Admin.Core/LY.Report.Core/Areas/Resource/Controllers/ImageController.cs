using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Auth;
using EonUp.Delivery.Core.Common.Configs;
using EonUp.Delivery.Core.Common.Helpers;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Model.Resource.Enum;
using EonUp.Delivery.Core.Service.Resource.ImgGallery;
using EonUp.Delivery.Core.Service.Resource.ImgGallery.Input;
using EonUp.Delivery.Core.Service.Resource.ImgGallery.Output;
using EonUp.Delivery.Core.Util.Common;
using EonUp.Delivery.Core.Util.Tool;
using EonUp.Delivery.Core.Common.Input;

namespace EonUp.Delivery.Core.Areas.Resource.Controllers
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
        /// 查询分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetListGetPageList(PageInput<ResImgGalleryGetInput> input)
        {
            return await _resImgGalleryService.GetPageListAsync(input);
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
            var checkRes = await _resImgGalleryService.GetOneAsync(new ResImgGalleryGetInput{ImgMd5 = imgMd5, UserId = _user.UserId});
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

        /// <summary>
        ///删除
        /// </summary>
        /// <param name="imgId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(string imgId)
        {
            return await _resImgGalleryService.SoftDeleteAsync(imgId);
        }

        /// <summary>
        ///批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        {
            return await _resImgGalleryService.BatchSoftDeleteAsync(ids);
        }
    }

 
}
