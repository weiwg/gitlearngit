using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Sales.Enum;

namespace LY.Report.Core.Service.Sales.Coupon.Input
{
    /// <summary>
    /// 查询
    /// </summary>
  public  class SalesCouponGetInput
    {
        /// <summary>
        /// 优惠券Id
        /// </summary>
        public string CouponId { get; set; }
        /// <summary>
        /// 优惠券名称
        /// </summary>
         public string CouponName { get; set; }

        /// <summary>
        /// 优惠券类型
        /// </summary>
        public CouponType CouponType { get; set; }

        /// <summary>
        /// 生效方式
        /// </summary>
        public CouponEffectiveType EffectiveType { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public IsActive IsActive { get; set; }

    }
}
