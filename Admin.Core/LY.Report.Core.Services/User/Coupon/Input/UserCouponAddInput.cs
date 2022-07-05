using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.User.Coupon.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class UserCouponAddInput
    {
        /// <summary>
        /// 优惠券ID
        /// </summary>
        [Required(ErrorMessage = "优惠券ID")]
        public string CouponId { get; set; }
    }
}
