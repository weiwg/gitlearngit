using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.User.Coupon;
using LY.Report.Core.Service.User.Coupon.Input;

namespace LY.Report.Core.Areas.Mobile.V1.User.Controllers
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
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<UserCouponGetInput> model)
        {
            return await _userCouponService.GetPageListAsync(model);
        }
        #endregion
    }
}
