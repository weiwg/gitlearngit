using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.User.Enum;
using FreeSql.DataAnnotations;
using System;
using System.ComponentModel;
using LY.Report.Core.Model.Sales.Enum;

namespace LY.Report.Core.Model.User
{
    /// <summary>
    /// 用户优惠券
    /// </summary>
    [Table(Name = "T_User_Coupon")]
    [Index("idx_{tablename}_01", nameof(CouponRecordId), true)]
    public class UserCoupon : EntityTenantFull
    {
        /// <summary>
        /// 优惠券记录Id
        /// </summary>
        [Description("优惠券记录Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string CouponRecordId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Description("用户Id")]
        [Column(IsNullable = false)]
        public string UserId { get; set; }

        /// <summary>
        /// 优惠券Id
        /// </summary>
        [Description("优惠券Id")]
        [Column(IsNullable = false)]
        public string CouponId { get; set; }

        /// <summary>
        /// 优惠券名称
        /// </summary>
        [Description(" 优惠券名称")]
        [Column(StringLength = 50, IsNullable = false)]
        public string CouponName { get; set; }

        /// <summary>
        /// 优惠券状态
        /// </summary>
        [Description("优惠券状态")]
        public UserCouponStatus CouponStatus { get; set; }

        /// <summary>
        /// 优惠券类型
        /// </summary>
        [Description("优惠券类型")]
        public CouponType CouponType { get; set; }

        /// <summary>
        /// 优惠券条件(付款金额)
        /// </summary>
        [Description("优惠券条件")]
        [Column(Precision = 12, Scale = 2)]
        public decimal CouponCondition { get; set; }

        /// <summary>
        /// 优惠券内容(优惠金额)
        /// </summary>
        [Description("优惠券内容")]
        [Column(Precision = 12, Scale = 2)]
        public decimal CouponContent { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        [Description("生效时间")]
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        [Description("失效时间")]
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remark { get; set; }
    }
}
