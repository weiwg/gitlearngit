using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Sales.Coupon;
using LY.Report.Core.Service.Sales.Coupon.Input;

namespace LY.Report.Core.Areas.Mobile.V1.Sales.Controllers
{
    /// <summary>
    /// 优惠券配置
    /// </summary>
    public class SalesCouponController : BaseAreaController
    {
        private readonly ISalesCouponService  _salesCouponService;

        public SalesCouponController(ISalesCouponService salesCouponService)
        {
            _salesCouponService = salesCouponService;
        }

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="couponId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string couponId)
        {
            return await _salesCouponService.GetOneAsync(couponId);
        }
    }
}
