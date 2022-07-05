using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Sales.Enum;
using FreeSql.DataAnnotations;
using System;
using System.ComponentModel;
using LY.Report.Core.Model.BaseEnum;

namespace LY.Report.Core.Model.Sales
{
    /// <summary>
    /// 优惠券
    /// </summary>
    [Table(Name = "T_Sales_Coupon")]
    [Index("idx_{tablename}_01", nameof(CouponId), true)]
    public class SalesCoupon : EntityTenantFull
    {
        /// <summary>
        /// 优惠券Id
        /// </summary>
        [Description("优惠券Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string CouponId { get; set; }

        /// <summary>
        ///  优惠券名称
        /// </summary>
        [Description(" 优惠券名称")]
        [Column(StringLength = 50, IsNullable = false)]
        public string CouponName { get; set; }

        /// <summary>
        /// 优惠券类型
        /// </summary>
        [Description("优惠券类型")]
        public CouponType CouponType { get; set; }

        /// <summary>
        /// 优惠券条件
        /// </summary>
        [Description("优惠券条件")]
        [Column(Precision = 12, Scale = 2)]
        public decimal CouponCondition { get; set; }

        /// <summary>
        ///  优惠券内容(优惠金额)
        /// </summary>
        [Description(" 优惠券内容")]
        [Column(Precision = 12, Scale = 2)]
        public decimal CouponContent { get; set; }

        /// <summary>
        /// 生效方式
        /// </summary>
        [Description("生效方式")]
        public CouponEffectiveType EffectiveType { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        [Description("生效时间")]
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        [Description("失效时间")]
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// 有效时间(分钟)
        /// </summary>
        [Description("有效时间")]
        public int EffectiveTime { get; set; }

        /// <summary>
        /// 发行数量
        /// </summary>
        [Description("发行数量")]
        [Column(IsNullable = false)]
        public int PublishCount { get; set; }

        /// <summary>
        /// 限领数量
        /// </summary>
        [Description(" 限领数量")]
        public int LimitCount { get; set; }

        /// <summary>
        /// 可领数量
        /// </summary>
        [Description("可领数量")]
        public int RemainCount { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        [Description("是否有效")]
        public IsActive IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        [Column(StringLength = 100)]
        public string Remark { get; set; }
    }
}
