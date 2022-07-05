using FreeSql;
using System;
using System.Threading.Tasks;
using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Sales;
using LY.Report.Core.Model.Sales.Enum;
using LY.Report.Core.Model.User;
using LY.Report.Core.Model.User.Enum;
using LY.Report.Core.Repository.Sales.Coupon;
using LY.Report.Core.Repository.User.RedPack;
using LY.Report.Core.Service.Base.Service;
using LY.Report.Core.Service.User.Coupon.Input;
using LY.Report.Core.Service.User.Coupon.Output;
using LY.Report.Core.Util.Common;

namespace LY.Report.Core.Service.User.Coupon
{
   public class UserCouponService: BaseService, IUserCouponService
    {
        private readonly IUserCouponRepository _userCouponRepository;
        private readonly ISalesCouponRepository _salesCouponRepository;
        private readonly LogHelper _logger = new LogHelper("UserCouponService");

        public UserCouponService(IUserCouponRepository userCouponRepository, ISalesCouponRepository salesCouponRepository)
        {
            _userCouponRepository = userCouponRepository;
            _salesCouponRepository = salesCouponRepository;
        }

        #region 添加
        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> AddAsync(UserCouponAddInput input)
        {
            if (string.IsNullOrEmpty(User?.UserId))
            {
                return ResponseOutput.NotOk("未登录！");
            }

            var coupon = await _salesCouponRepository.GetOneAsync<SalesCoupon>(t => t.CouponId == input.CouponId);
            if (coupon == null || coupon.CouponId.IsNull())
            {
                return ResponseOutput.NotOk("优惠券不存在");
            }
            if (coupon.EffectiveType == CouponEffectiveType.TimeRange && coupon.ExpiryDate < DateTime.Now)
            {
                return ResponseOutput.NotOk("优惠券已失效");
            }
            if (coupon.RemainCount <= 0)
            {
                return ResponseOutput.NotOk("优惠券已领完");
            }

            if (coupon.LimitCount != 0)
            {
                var userCouponCount = await _userCouponRepository.Select.Where(t => t.UserId == User.UserId && t.CouponId == input.CouponId).CountAsync();
                if (userCouponCount >= coupon.LimitCount)
                {
                    return ResponseOutput.NotOk("优惠券领取已到上限！");
                }
            }
            var entity = Mapper.Map<UserCoupon>(coupon);
            entity.CouponRecordId = CommonHelper.GetGuidD;
            entity.CouponStatus = UserCouponStatus.Unused;
            entity.UserId = User.UserId;
            if (coupon.EffectiveType == CouponEffectiveType.ReceiveTime)
            {
                entity.EffectiveDate = DateTime.Now;
                entity.ExpiryDate = DateTime.Now.AddMinutes(coupon.EffectiveTime);
            }
            var id = (await _userCouponRepository.InsertAsync(entity)).Id;
            if (id.IsNull())
            {
                return ResponseOutput.NotOk("优惠券领取失败！");
            }

            coupon.RemainCount = coupon.RemainCount - 1;
            var res = await _salesCouponRepository.UpdateDiyAsync
                .Set(t => t.RemainCount - 1)
                .SetIf(coupon.RemainCount <= 1, t => t.IsActive, IsActive.No)
                .Where(t => t.CouponId == entity.CouponId && t.RemainCount > 0)
                .ExecuteAffrowsAsync();
            if (res < 0)
            {
                return ResponseOutput.NotOk("优惠券领取失败！");
            }
            return ResponseOutput.Ok("优惠券领取成功");
        }
        #endregion

        #region 查询
        public async Task<IResponseOutput> GetOneAsync(string couponId)
        {
            var result = await _userCouponRepository.GetOneAsync<UserCoupon>(couponId);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetOneAsync(UserCouponGetInput input)
        {
            if (User.UserId.IsNull())
            {
                return ResponseOutput.NotOk("未登录！");
            }
            var whereSelect = _userCouponRepository.Select
                .Where(t => t.UserId == User.UserId)
                .WhereIf(input.CouponStatus == UserCouponStatus.Unused, t => t.CouponStatus == UserCouponStatus.Unused && t.EffectiveDate <= DateTime.Now && t.ExpiryDate >= DateTime.Now)
                .WhereIf(input.CouponStatus == UserCouponStatus.Used, t => t.CouponStatus == UserCouponStatus.Used)
                .WhereIf(input.CouponStatus == UserCouponStatus.UnActivated, t => t.CouponStatus == UserCouponStatus.Unused && t.EffectiveDate > DateTime.Now)
                .WhereIf(input.CouponStatus == UserCouponStatus.Expired, t => t.ExpiryDate < DateTime.Now && t.CouponStatus != UserCouponStatus.Used);
            var result = await _userCouponRepository.GetOneAsync<UserCouponGetOutput>(whereSelect);
            return ResponseOutput.Data(result);
        }

        public async Task<IResponseOutput> GetPageListAsync(PageInput<UserCouponGetInput> pageInput)
        {
            if (User.UserId.IsNull())
            {
                return ResponseOutput.NotOk("未登录！");
            }
            var input = pageInput.GetFilter();

            var list = await _userCouponRepository.Select
                .Where(t => t.UserId == User.UserId)
                .WhereIf(input.CouponStatus == UserCouponStatus.Unused, t => t.CouponStatus == UserCouponStatus.Unused && t.EffectiveDate <= DateTime.Now && t.ExpiryDate >= DateTime.Now)
                .WhereIf(input.CouponStatus == UserCouponStatus.Used, t => t.CouponStatus == UserCouponStatus.Used)
                .WhereIf(input.CouponStatus == UserCouponStatus.UnActivated, t => t.CouponStatus == UserCouponStatus.Unused && t.EffectiveDate > DateTime.Now)
                .WhereIf(input.CouponStatus == UserCouponStatus.Expired, t => t.ExpiryDate < DateTime.Now && t.CouponStatus != UserCouponStatus.Used)
                .Count(out var total)
                .OrderByDescending(true, c => c.ExpiryDate)
                .Page(pageInput.CurrentPage, pageInput.PageSize)
                .ToListAsync<UserCouponListOutput>();
            var data = new PageOutput<UserCouponListOutput>
            {
                List = list,
                Total = total
            };
           return ResponseOutput.Data(data);
        }
        #endregion
        
        #region TimerJob
        [Transaction(Propagation = Propagation.Required)]
        public async Task<IResponseOutput> CheckUserCouponStatusTimerJob()
        {
            var res = await _userCouponRepository.UpdateDiyAsync
                .Set(t => t.CouponStatus, UserCouponStatus.Expired)
                .Where(t => t.ExpiryDate < DateTime.Now && t.CouponStatus == UserCouponStatus.Unused)
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
