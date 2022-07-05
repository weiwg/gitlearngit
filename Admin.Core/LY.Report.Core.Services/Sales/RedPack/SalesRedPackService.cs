using FreeSql;
using System;
using System.Threading.Tasks;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Sales;
using LY.Report.Core.Model.Sales.Enum;
using LY.Report.Core.Repository.Sales;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Sales.RedPack.Input;
using LY.Report.Core.Service.Sales.RedPack.Output;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.Sales.RedPack
{
    public class SalesRedPackService : BaseService, ISalesRedPackService
    {
        private readonly ISalesRedPackRepository _salesRedPackRepository;
        private readonly LogHelper _logger = new LogHelper("SalesRedPackService");

        public SalesRedPackService(ISalesRedPackRepository salesRedPackRepository)
        {
            _salesRedPackRepository = salesRedPackRepository;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(SalesRedPackAddInput input)
        {
            if (input.LimitCount > input.PublishCount)
            {
                return ResponseOutput.NotOk("限领数量不能大于发行数量");
            }

            if (input.EffectiveType == RedPackEffectiveType.TimeRange)
            {
                input.EffectiveTime = 0;
                if (input.ExpiryDate < input.EffectiveDate)
                {
                    var temp = input.ExpiryDate;
                    input.ExpiryDate = input.EffectiveDate;
                    input.EffectiveDate = temp;
                }
            }
            else if (input.EffectiveType == RedPackEffectiveType.ReceiveTime)
            {
                input.EffectiveDate = null;
                input.ExpiryDate = null;
                if (input.EffectiveTime <= 0)
                {
                    return ResponseOutput.NotOk("有效时间必须大于0分钟");
                }
            }
            var entity = Mapper.Map<SalesRedPack>(input);
            entity.RedPackId = CommonHelper.GetGuidD;
            entity.RemainCount = entity.PublishCount;
            var id = (await _salesRedPackRepository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateEntityAsync(SalesRedPackUpdateInput input)
        {
            if (input.LimitCount > input.PublishCount)
            {
                return ResponseOutput.NotOk("限领数量不能大于发行数量！");
            }
            var entity = await _salesRedPackRepository.GetAsync(input.RedPackId);
            if (string.IsNullOrEmpty(entity.RedPackId))
            {
                return ResponseOutput.NotOk("数据不存在！");
            }
            if (entity.PublishCount < input.PublishCount)
            {
                return ResponseOutput.NotOk("发行数量不可减少");
            }

            //增加新发行数量
            entity.RemainCount = input.PublishCount - entity.PublishCount + entity.RemainCount;
            Mapper.Map(input, entity);
            int res = await _salesRedPackRepository.UpdateAsync(entity);
            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改失败");
            }
            return ResponseOutput.Ok("修改成功");
        }
        #endregion

        #region 查询

        public async Task<IResponseOutput> GetOneAsync(string redPackId)
        {
            var result = await _salesRedPackRepository.GetOneAsync<SalesRedPackGetOutput>(redPackId);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(SalesRedPackGetInput input)
        {       
            var whereSelect = _salesRedPackRepository.Select
               .WhereIf(input.RedPackId.IsNotNull(), t => t.RedPackId == input.RedPackId)
               .WhereIf(input.RedPackName.IsNotNull(), t => t.RedPackName.Contains(input.RedPackName))
               .WhereIf(input.IsActive > 0, t => t.IsActive == input.IsActive)
               .WhereIf(input.EffectiveType > 0, t => t.EffectiveType == input.EffectiveType);
            var result = await _salesRedPackRepository.GetOneAsync<SalesRedPackGetOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<SalesRedPackGetInput> pageInput)
        {
            var input = pageInput.GetFilter();

            var list = await _salesRedPackRepository.Select
                .WhereIf(input.RedPackId.IsNotNull(), t => t.RedPackId == input.RedPackId)
                .WhereIf(input.RedPackName.IsNotNull(), t => t.RedPackName.Contains(input.RedPackName))
                .WhereIf(input.IsActive > 0, t => t.IsActive == input.IsActive)
                .WhereIf(input.EffectiveType > 0, t => t.EffectiveType == input.EffectiveType)
                .Count(out var total)
                .OrderByDescending(true, c => c.CreateDate)
                .Page(pageInput.CurrentPage, pageInput.PageSize)
                .ToListAsync<SalesRedPackGetOutput>();
            var data = new PageOutput<SalesRedPackGetOutput>
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        #endregion

        #region 删除
        public async Task<IResponseOutput> SoftDeleteAsync(string redPackId)
        {
            if (redPackId.IsNull())
            {
                return ResponseOutput.NotOk("参数错误");
            }
            var result = await _salesRedPackRepository.SoftDeleteAsync(redPackId);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(SalesRedPackDeleteInput input)
        {
            var result = false;
            if (input.RedPackId.IsNotNull())
            {
                result = (await _salesRedPackRepository.SoftDeleteAsync(t => t.RedPackId == input.RedPackId));
            }

            return ResponseOutput.Result(result);
        }


        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseOutput.NotOk("参数错误");
            }

            var result = await _salesRedPackRepository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
        #endregion

        #region TimerJob
        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> CheckSysRedPackStatusTimerJob()
        {
            var res = await _salesRedPackRepository.UpdateDiyAsync
                .Set(t => t.IsActive, IsActive.No)
                .Where(t =>
                    t.EffectiveType == RedPackEffectiveType.TimeRange &&
                    t.ExpiryDate < DateTime.Now &&
                    t.IsActive == IsActive.Yes)
                .ExecuteAffrowsAsync();
            if (res < 0)
            {
                _logger.Error($"处理红包状态错误:{res}");
            }

            return ResponseOutput.Ok("处理成功");
        }

        #endregion
    }
}
