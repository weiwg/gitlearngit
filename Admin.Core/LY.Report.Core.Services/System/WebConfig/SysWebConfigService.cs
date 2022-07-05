using AutoMapper;
using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.System;
using LY.Report.Core.Repository.System;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.System.WebConfig.Input;
using LY.Report.Core.Service.System.WebConfig.Output;

namespace LY.Report.Core.Service.System.WebConfig
{
    public class SysWebConfigService : BaseService, ISysWebConfigService
    {
        private readonly ISysWebConfigRepository _repository;
        public SysWebConfigService(ISysWebConfigRepository repository)
        {
            _repository = repository;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(SysWebConfigAddInput input)
        {
            var entity = Mapper.Map<SysWebConfig>(input);
            var id = (await _repository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateAsync(SysWebConfigUpdateInput input)
        {
            if (string.IsNullOrEmpty(input.ConfigId))
            {
                return ResponseOutput.NotOk();
            }

            int res = await _repository.UpdateDiyAsync
                .Set(t => t.ConfigId, "test")
                .Where(t => t.ConfigId == input.ConfigId)
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
            var result = await _repository.GetOneAsync<SysWebConfigGetOutput>(id);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(SysWebConfigGetInput input)
        {
            //var result = await _repository.GetOneAsync(t => t.ConfigId == input.ConfigId);//获取实体
            var result = await _repository.GetOneAsync<SysWebConfigGetOutput>(t => t.ConfigId == input.ConfigId);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<SysWebConfigGetInput> input)
        {
            var id = input.Filter?.ConfigId;

            long total;
            var list = await _repository.GetPageListAsync<SysWebConfig>(t => t.ConfigId == id, input.CurrentPage,input.PageSize, t => t.ConfigId, out total);

            var data = new PageOutput<SysWebConfig>()
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

        public async Task<IResponseOutput> SoftDeleteAsync(SysWebConfigDeleteInput input)
        {
            var result = false;
            if (string.IsNullOrEmpty(input.ConfigId))
            {
                result = (await _repository.SoftDeleteAsync(t => t.ConfigId == input.ConfigId));
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
