using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Sales.Coupon;
using LY.Report.Core.Service.Sales.Coupon.Input;

namespace LY.Report.Core.Areas.Sys.V1.Sales.Controllers
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

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<SalesCouponGetInput> model)
        {
            return await _salesCouponService.GetPageListAsync(model);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(SalesCouponAddInput input)
        {
            return await _salesCouponService.AddAsync(input);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(SalesCouponUpdateInput input)
        {
            return await _salesCouponService.UpdateEntityAsync(input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="couponId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(string couponId)
        {
            return await _salesCouponService.SoftDeleteAsync(couponId);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        {
            return await _salesCouponService.BatchSoftDeleteAsync(ids);
        }
    }
}
