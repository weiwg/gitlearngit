
using System;
using LY.Report.Core.Model.Sales.Enum;
using LY.Report.Core.Model.User.Enum;

namespace LY.Report.Core.Service.User.Coupon.Output
{
    public class UserCouponGetOutput
    {
        /// <summary>
        /// 优惠券Id
        /// </summary>
        public string CouponId { get; set; }

        /// <summary>
        /// 优惠券状态
        /// </summary>
        public UserCouponStatus CouponStatus { get; set; }

        /// <summary>
        /// 优惠券类型
        /// </summary>
        public CouponType CouponType { get; set; }

        /// <summary>
        /// 优惠券名称
        /// </summary>
        public string CouponName { get; set; }

        /// <summary>
        /// 优惠券条件(付款金额)
        /// </summary>
        public decimal CouponCondition { get; set; }

        /// <summary>
        /// 优惠券内容(优惠金额)
        /// </summary>
        public decimal CouponContent { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
