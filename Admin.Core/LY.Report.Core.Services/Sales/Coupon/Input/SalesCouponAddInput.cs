using System;
using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Sales.Enum;
using LY.Report.Core.Util.Attributes.Validation;

namespace LY.Report.Core.Service.Sales.Coupon.Input
{
    /// <summary>
    /// 添加
    /// </summary>
   public class SalesCouponAddInput
    {
        /// <summary>
        /// 优惠券名称
        /// </summary>
        [Display(Name = "优惠券名称")]
        [Required(ErrorMessage = "优惠券名称不能为空"), StringLength(50, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string CouponName { get; set; }

        /// <summary>
        /// 优惠券类型
        /// </summary>
        [Display(Name = "优惠券类型")]
        [Required(ErrorMessage = "优惠券类型不能为空")]
        public CouponType CouponType { get; set; }

        /// <summary>
        /// 优惠券条件
        /// </summary>
        [Display(Name = "优惠券条件")]
        [CustomPrice]
        public decimal CouponCondition { get; set; }

        /// <summary>
        ///  优惠券内容(优惠金额)
        /// </summary>
        [Display(Name = "优惠券内容")]
        [CustomPrice]
        public decimal CouponContent { get; set; }

        /// <summary>
        /// 生效方式
        /// </summary>
        [Display(Name = "生效方式")]
        [Required(ErrorMessage = "生效方式不能为空")]
        public CouponEffectiveType EffectiveType { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        [Display(Name = "生效时间")]
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        [Display(Name = "失效时间")]
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// 有效时间(分钟)
        /// </summary>
        [Display(Name = "有效时间(分钟)")]
        [CustomNumber]
        [Required(ErrorMessage = "有效时间不能为空")]
        public int EffectiveTime { get; set; }

        /// <summary>
        /// 发行数量
        /// </summary>
        [Display(Name = "发行数量")]
        [CustomNumber]
        [Required(ErrorMessage = "发行数量不能为空")]
        public int PublishCount { get; set; }

        /// <summary>
        /// 限领数量(0为无限制)
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
