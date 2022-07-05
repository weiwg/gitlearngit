using FreeSql;
using System;
using System.Threading.Tasks;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Sales.Enum;
using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.Sales;
using LY.Report.Core.Model.User;
using LY.Report.Core.Model.User.Enum;
using LY.Report.Core.Repository.Sales;
using LY.Report.Core.Repository.User.RedPack;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.User.RedPack.Input;
using LY.Report.Core.Service.User.RedPack.Output;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.User.RedPack
{
   public class UserRedPackService: BaseService, IUserRedPackService
    {
        private readonly IUserRedPackRepository _userRedPackRepository;
        private readonly ISalesRedPackRepository _salesRedPackRepository;
        private readonly LogHelper _logger = new LogHelper("UserRedPackService");

        public UserRedPackService(IUserRedPackRepository userRedPackRepository, ISalesRedPackRepository salesRedPackRepository)
        {
            _userRedPackRepository = userRedPackRepository;
            _salesRedPackRepository = salesRedPackRepository;
        }

        #region 添加
        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> AddAsync(UserRedPackAddInput input)
        {
            if (string.IsNullOrEmpty(User?.UserId))
            {
                return ResponseOutput.NotOk("未登录！");
            }

            var redPack = await _salesRedPackRepository.GetOneAsync<SalesRedPack>(t => t.RedPackId == input.RedPackId);
            if (redPack == null || redPack.RedPackId.IsNull())
            {
                return ResponseOutput.NotOk("红包不存在");
            }
            if (redPack.EffectiveType == RedPackEffectiveType.TimeRange && redPack.ExpiryDate < DateTime.Now)
            {
                return ResponseOutput.NotOk("红包已失效");
            }
            if (redPack.RemainCount <= 0)
            {
                return ResponseOutput.NotOk("红包已领完");
            }

            if (redPack.LimitCount != 0)
            {
                var userRedPackCount = await _userRedPackRepository.Select.Where(t => t.UserId == User.UserId && t.RedPackId == input.RedPackId).CountAsync();
                if (userRedPackCount >= redPack.LimitCount)
                {
                    return ResponseOutput.NotOk("红包领取已到上限！");
                }
            }

            var entity = Mapper.Map<UserRedPack>(redPack);
            entity.RedPackRecordId= CommonHelper.GetGuidD;
            entity.RemainAmount = redPack.RedPackAmount;
            entity.UserId = User.UserId;
            if (redPack.EffectiveType == RedPackEffectiveType.ReceiveTime)
            {
                entity.EffectiveDate = DateTime.Now;
                entity.ExpiryDate = DateTime.Now.AddMinutes(redPack.EffectiveTime);
            }
            var id = (await _userRedPackRepository.InsertAsync(entity)).Id;
            if (id.IsNull())
            {
                return ResponseOutput.NotOk("红包领取失败！");
            }

            redPack.RemainCount = redPack.RemainCount - 1;
            var res = await _salesRedPackRepository.UpdateDiyAsync
                .Set(t => t.RemainCount - 1)
                .SetIf(redPack.RemainCount <= 1, t => t.IsActive, IsActive.No)
                .Where(t => t.RedPackId == entity.RedPackId)
                .ExecuteAffrowsAsync();
            if (res < 0)
            {
                return ResponseOutput.NotOk("红包领取失败！");
            }
            return ResponseOutput.Ok("领取成功");
        }
        #endregion
        
        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string redPackId)
        {
            var result = await _userRedPackRepository.GetOneAsync<UserRedPack>(redPackId);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(UserRedPackGetInput input)
        {
            if (User.UserId.IsNull())
            {
                return ResponseOutput.NotOk("未登录！");
            }

            var whereSelect = _userRedPackRepository.Select
                .Where(t => t.UserId == User.UserId)
                .WhereIf(input.RedPackStatus == UserRedPackStatus.Unused, t => t.RedPackStatus == UserRedPackStatus.Unused && t.EffectiveDate <= DateTime.Now && t.ExpiryDate >= DateTime.Now)
                .WhereIf(input.RedPackStatus == UserRedPackStatus.Used, t => t.RedPackStatus == UserRedPackStatus.Used || t.RemainAmount <= 0)
                .WhereIf(input.RedPackStatus == UserRedPackStatus.UnActivated, t => t.RedPackStatus == UserRedPackStatus.Unused && t.EffectiveDate > DateTime.Now)
                .WhereIf(input.RedPackStatus == UserRedPackStatus.Expired, t => t.ExpiryDate < DateTime.Now && t.RedPackStatus != UserRedPackStatus.Used);
            var result = await _userRedPackRepository.GetOneAsync<UserRedPackGetOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<UserRedPackGetInput> pageInput)
        {
            if (User.UserId.IsNull())
            {
                return ResponseOutput.NotOk("未登录！");
            }
            var input = pageInput.GetFilter();

            var list = await _userRedPackRepository.Select
                .Where(t => t.UserId == User.UserId)
                .WhereIf(input.RedPackStatus == UserRedPackStatus.Unused, t=>t.RedPackStatus == UserRedPackStatus.Unused && t.EffectiveDate <= DateTime.Now && t.ExpiryDate >= DateTime.Now)
                .WhereIf(input.RedPackStatus == UserRedPackStatus.Used, t => t.RedPackStatus == UserRedPackStatus.Used || t.RemainAmount <= 0)
                .WhereIf(input.RedPackStatus == UserRedPackStatus.UnActivated, t => t.RedPackStatus == UserRedPackStatus.Unused && t.EffectiveDate > DateTime.Now)
                .WhereIf(input.RedPackStatus == UserRedPackStatus.Expired, t => t.ExpiryDate < DateTime.Now && t.RedPackStatus != UserRedPackStatus.Used)
                .Count(out var total)
                .OrderByDescending(true, c => c.ExpiryDate)
                .Page(pageInput.CurrentPage, pageInput.PageSize)
                .ToListAsync<UserRedPackListOutput>();
            var data = new PageOutput<UserRedPackListOutput>
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        #endregion

        #region TimerJob
        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> CheckUserRedPackStatusTimerJob()
        {
            var res = await _userRedPackRepository.UpdateDiyAsync
                .Set(t => t.RedPackStatus, UserRedPackStatus.Expired)
                .Where(t => (t.ExpiryDate < DateTime.Now || t.RemainAmount <= 0) && t.RedPackStatus == UserRedPackStatus.Unused)
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
