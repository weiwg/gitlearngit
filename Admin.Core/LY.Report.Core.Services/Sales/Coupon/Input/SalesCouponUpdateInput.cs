using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Util.Attributes.Validation;

namespace LY.Report.Core.Service.Sales.Coupon.Input
{
    /// <summary>
    /// 修改
    /// </summary>
   public class SalesCouponUpdateInput
    {
        /// <summary>
        /// 优惠券Id
        /// </summary>
        [Required(ErrorMessage = "优惠券Id不能为空")]
        public string CouponId { get; set; }

        /// <summary>
        /// 发行数量
        /// </summary>
        [Display(Name = "发行数量")]
        [CustomNumber]
        [Required(ErrorMessage = "发行数量不能为空")]
        public int PublishCount { get; set; }

        /// <summary>
        /// 限领数量
        /// </summary>
        [Display(Name = "限领数量")]
        [CustomNumber]
        [Required(ErrorMessage = "限领数量不能为空")]
        public int LimitCount { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        [Display(Name = "是否有效")]
        [Required(ErrorMessage = "是否有效不能为空")]
        public IsActive IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [StringLength(100, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 0)]
        public string Remark { get; set; }
    }
}
