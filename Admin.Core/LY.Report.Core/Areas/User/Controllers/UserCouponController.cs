using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.User.Coupon;
using EonUp.Delivery.Core.Service.User.Coupon.Input;

namespace EonUp.Delivery.Core.Areas.User.Controllers
{
    /// <summary>
    /// 用户优惠券
    /// </summary>
    public class UserCouponController : BaseAreaController
    {
        private readonly IUserCouponService  _userCouponService;

        public UserCouponController(IUserCouponService userCouponService)
        {
            _userCouponService = userCouponService;
        }

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(UserCouponAddInput input)
        {
            return await _userCouponService.AddAsync(input);
        }
        #endregion

        #region 查询
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="couponId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string couponId)
        {
            return await _userCouponService.GetOneAsync(couponId);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<UserCouponGetInput> model)
        {
            return await _userCouponService.GetPageListAsync(model);
        }
        #endregion
    }
}
