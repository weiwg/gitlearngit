using System.ComponentModel;

namespace LY.Report.Core.Model.Sales.Enum
{
    /// <summary>
    /// 优惠券类型
    /// </summary>
    public enum CouponType
    {
        /// <summary>
        /// 满减
        /// </summary>
        [Description("满减")]
        Reduction = 1,
        /// <summary>
        /// 现金券
        /// </summary>
        [Description("现金券")]
        CashCoupon = 2
    }

    /// <summary>
    /// 生效方式
    /// </summary>
    public enum CouponEffectiveType
    {
        /// <summary>
        /// 时间范围
        /// </summary>
        [Description("时间范围")]
        TimeRange = 1,
        /// <summary>
        /// 领取时间(分钟)
        /// </summary>
        [Description("领取时间")]
        ReceiveTime = 2
    }

}
