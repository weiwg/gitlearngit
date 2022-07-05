using FreeSql;
using System;
using System.Threading.Tasks;
using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Sales;
using LY.Report.Core.Model.Sales.Enum;
using LY.Report.Core.Repository.Sales.Coupon;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.Sales.Coupon.Input;
using LY.Report.Core.Service.Sales.Coupon.Output;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.Sales.Coupon
{
    public class SalesCouponService: BaseService, ISalesCouponService
    {
        private readonly ISalesCouponRepository _salesCouponRepository;
        private readonly LogHelper _logger = new LogHelper("SalesCouponService");

        public SalesCouponService(ISalesCouponRepository salesCouponRepository)
        {
            _salesCouponRepository = salesCouponRepository;
        }

        #region 添加
        public async Task<IResponseOutput> AddAsync(SalesCouponAddInput input)
        {
            #region 参数校验
            if (input.LimitCount > input.PublishCount)
            {
                return ResponseOutput.NotOk("限领数量不能大于发行数量");
            }
            if (input.CouponCondition <= input.CouponContent) 
            {
                return ResponseOutput.NotOk("优惠金额需小于优惠条件");
            }
            if (input.EffectiveType == CouponEffectiveType.TimeRange)
            {
                if (input.ExpiryDate == null || input.EffectiveDate == null)
                {
                    return ResponseOutput.NotOk("生效范围错误");
                }
                input.EffectiveTime = 0;
                if (input.ExpiryDate < input.EffectiveDate)
                {
                    var temp = input.ExpiryDate;
                    input.ExpiryDate = input.EffectiveDate;
                    input.EffectiveDate = temp;
                }
            }
            else if (input.EffectiveType == CouponEffectiveType.ReceiveTime)
            {
                input.EffectiveDate = null;
                input.ExpiryDate = null;
                if (input.EffectiveTime <= 0)
                {
                    return ResponseOutput.NotOk("有效时间必须大于0分钟");
                }
            }
            #endregion

            var entity = Mapper.Map<SalesCoupon>(input);
            entity.CouponId = CommonHelper.GetGuidD;
            entity.RemainCount = entity.PublishCount;
            var id = (await _salesCouponRepository.InsertAsync(entity)).Id;

            return ResponseOutput.Result(id.IsNotNull());
        }
        #endregion

        #region 修改
        public async Task<IResponseOutput> UpdateEntityAsync(SalesCouponUpdateInput input)
        {
            if (input.LimitCount > input.PublishCount)
            {
                return ResponseOutput.NotOk("限领数量不能大于发行数量！");
            }
            var entity = await _salesCouponRepository.GetAsync(input.CouponId);
            if (string.IsNullOrEmpty(entity.CouponId))
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
            int res = await _salesCouponRepository.UpdateAsync(entity);
            if (res <= 0)
            {
                return ResponseOutput.NotOk("修改失败");
            }
            return ResponseOutput.Ok("修改成功");
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string couponId)
        {
            var result = await _salesCouponRepository.GetOneAsync<SalesCouponGetOutput>(couponId);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(SalesCouponGetInput input)
        {
            var whereSelect = _salesCouponRepository.Select
              .WhereIf(input.CouponId.IsNotNull(), t => t.CouponId == input.CouponId)
              .WhereIf(input.CouponName.IsNotNull(), t => t.CouponName.Contains(input.CouponName))
              .WhereIf(input.IsActive > 0, t => t.IsActive == input.IsActive)
              .WhereIf(input.EffectiveType > 0, t => t.EffectiveType == input.EffectiveType)
              .WhereIf(input.CouponType > 0, t => t.CouponType == input.CouponType);
            var result = await _salesCouponRepository.GetOneAsync<SalesCouponGetOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<SalesCouponGetInput> pageInput)
        {
            var input = pageInput.GetFilter();

            var list = await _salesCouponRepository.Select
                .WhereIf(input.CouponId.IsNotNull(), t => t.CouponId == input.CouponId)
                .WhereIf(input.CouponName.IsNotNull(), t => t.CouponName.Contains(input.CouponName))
                .WhereIf(input.IsActive > 0, t => t.IsActive == input.IsActive)
                .WhereIf(input.EffectiveType > 0, t => t.EffectiveType == input.EffectiveType)
                .WhereIf(input.CouponType > 0, t => t.CouponType == input.CouponType)
                .Count(out var total)
                .OrderByDescending(true, c => c.CreateDate)
                .Page(pageInput.CurrentPage, pageInput.PageSize)
                .ToListAsync<SalesCouponGetOutput>();
            var data = new PageOutput<SalesCouponGetOutput>
            {
                List = list,
                Total = total
            };

            return ResponseOutput.Data(data);
        }
        #endregion

        #region 删除
        public async Task<IResponseOutput> SoftDeleteAsync(string couponId)
        {
            if (couponId.IsNull())
            {
                return ResponseOutput.NotOk("参数错误");
            }
            var result = await _salesCouponRepository.SoftDeleteAsync(couponId);
            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(SalesCouponDeleteInput input)
        {
            var result = false;
            if (input.CouponId.IsNotNull())
            {
                result = (await _salesCouponRepository.SoftDeleteAsync(t => t.CouponId == input.CouponId));
            }

            return ResponseOutput.Result(result);
        }
        public async Task<IResponseOutput> BatchSoftDeleteAsync(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return ResponseOutput.NotOk("参数错误");
            }

            var result = await _salesCouponRepository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
        #endregion

        #region TimerJob
        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> CheckSysCouponStatusTimerJob()
        {
            var res = await _salesCouponRepository.UpdateDiyAsync
                .Set(t => t.IsActive, IsActive.No)
                .Where(t =>
                    t.EffectiveType == CouponEffectiveType.TimeRange &&
                    t.ExpiryDate < DateTime.Now &&
                    t.IsActive == IsActive.Yes)
                .ExecuteAffrowsAsync();
            if (res < 0)
            {
                _logger.Error($"处理优惠券状态错误:{res}");
            }
            return ResponseOutput.Ok("处理成功");
        }

        #endregion
    }
}
