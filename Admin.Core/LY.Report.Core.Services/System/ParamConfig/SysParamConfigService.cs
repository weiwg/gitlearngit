using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.System;
using LY.Report.Core.Repository.System;
using LY.Report.Core.Service.System.ParamConfig.Input;
using LY.Report.Core.Service.System.ParamConfig.Output;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.System.ParamConfig
{
    public class SysParamConfigService : ISysParamConfigService
    {
        private readonly IMapper _mapper;
        private readonly ISysParamConfigRepository _repository;
        public SysParamConfigService(IMapper mapper, ISysParamConfigRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(SysParamConfigAddInput input)
        {
            input.ConfigId = CommonHelper.GetGuidD;
            var entity = _mapper.Map<SysParamConfig>(input);
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateAsync(SysParamConfigUpdateInput input)
        {
            if (string.IsNullOrEmpty(input.Id))
            {
                return ResponseOutput.NotOk();
            }

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.Id, "test")
                .Where(t => t.Id == input.Id)
                .ExecuteAffrowsAsync();

            if (res <= 0)
            {
                return ResponseOutput.NotOk();
            }
            return ResponseOutput.Ok();
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string id)
        {
            var result = await _repository.GetOneAsync<SysParamConfigGetOutput>(id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(SysParamConfigGetInput input)
        {
            //var result = await _repository.GetOneAsync(t => t.ParamKey == input.ParamKey);//获取实体
            var result = await _repository.GetOneAsync<SysParamConfigGetOutput>(t => t.ParamKey == input.ParamKey);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<SysParamConfigGetInput> input)
        {
            var id = input.Filter?.Id;

            long total;
            var list = await _repository.GetPageListAsync<SysParamConfig>(t => t.Id == id, input.CurrentPage,input.PageSize, t => t.Id, out total);

            var data = new PageOutput<SysParamConfig>()
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        #endregion

        #region 删除
        public async Task<IResponseOutput> SoftDeleteAsync(string id)
        {
            var result = await _repository.SoftDeleteAsync(id);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(SysParamConfigDeleteInput input)
        {
            var result = false;
            if (string.IsNullOrEmpty(input.ParamKey))
            {
                result = (await _repository.SoftDeleteAsync(t => t.ParamKey == input.ParamKey));
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
