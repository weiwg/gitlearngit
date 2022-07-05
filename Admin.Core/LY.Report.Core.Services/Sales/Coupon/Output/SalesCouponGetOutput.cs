
using System;
using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Model.Sales.Enum;

namespace LY.Report.Core.Service.Sales.Coupon.Output
{
   public class SalesCouponGetOutput
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
        /// 优惠券条件
        /// </summary>
        
        public decimal CouponCondition { get; set; }

        /// <summary>
        ///  优惠券内容(优惠金额)
        /// </summary>
      
        public decimal CouponContent { get; set; }

        /// <summary>
        /// 生效方式
        /// </summary>
        public CouponEffectiveType EffectiveType { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// 有效时间(分钟)
        /// </summary>
        public int EffectiveTime { get; set; }

        /// <summary>
        /// 发行数量
        /// </summary>
        public int PublishCount { get; set; }

        /// <summary>
        /// 可领数量
        /// </summary>
        public int RemainCount { get; set; }

        /// <summary>
        /// 限领数量(0为无限制)
        /// </summary>

        public int LimitCount { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public IsActive IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
