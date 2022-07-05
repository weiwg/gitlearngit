using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Resource;
using LY.Report.Core.Repository.Resource;
using LY.Report.Core.Service.Resource.ImgData.Input;
using LY.Report.Core.Service.Resource.ImgData.Output;

namespace LY.Report.Core.Service.Resource.ImgData
{
    public class ResImgDataService : IResImgDataService
    {
        private readonly IMapper _mapper;
        private readonly IResImgDataRepository _repository;
        public ResImgDataService(IMapper mapper, IResImgDataRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(ResImgDataAddInput input)
        {
            var entity = _mapper.Map<ResImgData>(input);
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateAsync(ResImgDataUpdateInput input)
        {
            if (string.IsNullOrEmpty(input.Id))
            {
                return ResponseOutput.NotOk();
            }

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.ImgData, input.ImgData)
                .Where(t => t.ImgMd5 == input.ImgMd5)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk();
            }
            return ResponseOutput.Ok();
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string imgMd5)
        {
            var result = await _repository.GetOneAsync<ResImgDataGetOutput>(imgMd5);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(ResImgDataGetInput input)
        {
            //var result = await _repository.GetOneAsync(t => t.ImgMd5 == input.ImgMd5);//获取实体
            var result = await _repository.GetOneAsync<ResImgDataGetOutput>(t => t.ImgMd5 == input.ImgMd5);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<ResImgDataGetInput> input)
        {
            var imgMd5 = input.Filter?.ImgMd5;

            long total;
            var list = await _repository.GetPageListAsync<ResImgData>(t => t.ImgMd5 == imgMd5, input.CurrentPage,input.PageSize, t => t.ImgMd5, out total);

            var data = new PageOutput<ResImgData>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }        

        #endregion

        #region 删除        

        public async Task<IResponseOutput> SoftDeleteAsync(string imgMd5)
        {
            var result = await _repository.SoftDeleteAsync(imgMd5);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(ResImgDataDeleteInput input)
        {
            var result = false;
            if (string.IsNullOrEmpty(input.ImgMd5))
            {
                result = (await _repository.SoftDeleteAsync(t => t.ImgMd5 == input.ImgMd5));
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
