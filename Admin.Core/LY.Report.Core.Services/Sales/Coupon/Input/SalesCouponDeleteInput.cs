
using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Sales.Coupon.Input
{
    /// <summary>
    /// 删除
    /// </summary>
   public class SalesCouponDeleteInput
    {
       
        /// <summary>
        /// 优惠券Id
        /// </summary>
        [Required(ErrorMessage = "优惠券Id不能为空")]
        public string CouponId { get; set; }
    }
}
