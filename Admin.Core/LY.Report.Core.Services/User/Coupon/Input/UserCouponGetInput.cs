
using LY.Report.Core.Model.User.Enum;
using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.User.Coupon.Input
{
    /// <summary>
    /// 查询
    /// </summary>
   public class UserCouponGetInput
    {
        /// <summary>
        /// 优惠券状态
        /// </summary>
        [Required(ErrorMessage = "优惠券状态")]
        public UserCouponStatus CouponStatus { get; set; }
    }
}
