using AutoMapper;
using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Resource;
using LY.Report.Core.Repository.Resource;
using LY.Report.Core.Service.Resource.ImgGallery.Input;
using LY.Report.Core.Service.Resource.ImgGallery.Output;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.Resource.ImgGallery
{
    public class ResImgGalleryService : IResImgGalleryService
    {
        private readonly IMapper _mapper;
        private readonly IResImgGalleryRepository _repository;
        public ResImgGalleryService(IMapper mapper, IResImgGalleryRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        #region Ìí¼Ó
        public async Task<IResponseOutput> AddAsync(ResImgGalleryAddInput input)
        {
            var entity = _mapper.Map<ResImgGallery>(input);
            entity.ImgId = CommonHelper.GetGuidD;
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region ²éÑ¯
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var result = await _repository.GetOneAsync<ResImgGalleryGetOutput>(id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(ResImgGalleryGetInput input)
        {
            var whereSelect = _repository.Select
                .WhereIf(input.ImgMd5.IsNotNull(), t => t.ImgMd5 == input.ImgMd5)
                .WhereIf(input.ImgCategory > 0, t => t.ImgCategory == input.ImgCategory)
                .WhereIf(input.FileName.IsNotNull(), t => t.FileName == input.FileName)
                .WhereIf(input.UserId.IsNotNull(), t => t.FileName == input.UserId);
            var result = await _repository.GetOneAsync<ResImgGalleryGetOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<ResImgGalleryGetInput> input)
        {
            var imgMd5 = input.Filter?.ImgMd5;
            var imgCategory = input.Filter?.ImgCategory;
            var fileName = input.Filter?.FileName;
            var userId = input.Filter?.UserId;

            var list = await _repository.Select
                .WhereIf(imgMd5.IsNotNull(), t => t.ImgMd5 == imgMd5)
                .WhereIf(imgCategory > 0, t => t.ImgCategory == imgCategory)
                .WhereIf(fileName.IsNotNull(), t => t.FileName == fileName)
                .WhereIf(userId.IsNotNull(), t => t.FileName == userId)
                .Count(out var total)
                .OrderByDescending(true, c => c.CreateDate)
                .Page(input.CurrentPage, input.PageSize)
                .ToListAsync<ResImgGalleryListOutput>();
            var data = new PageOutput<ResImgGalleryListOutput>
            {
               List = list,
               Total = total
            };

            return ResponseOutput.Data(data);
        }
        #endregion

        #region É¾³ý        

        public async Task<IResponseOutput> SoftDeleteAsync(string imgId)
        {
            var result = await _repository.SoftDeleteAsync(imgId);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(ResImgGalleryDeleteInput input)
        {
            var result = false;
            if (string.IsNullOrEmpty(input.ImgId))
            {
                result = (await _repository.SoftDeleteAsync(t => t.ImgMd5 == input.ImgId));
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            var result = await _repository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
        #endregion
    }
}
