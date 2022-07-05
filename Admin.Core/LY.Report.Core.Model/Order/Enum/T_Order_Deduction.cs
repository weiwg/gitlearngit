using System.ComponentModel;

namespace LY.Report.Core.Model.Order.Enum
{
    /// <summary>
    /// 抵扣类型
    /// </summary>
    public enum DeductionType
    {
        /// <summary>
        /// 优惠券
        /// </summary>
        [Description("优惠券")]
        Coupon = 1,
        /// <summary>
        /// 积分
        /// </summary>
        [Description("积分")]
        Integral = 2,
        /// <summary>
        /// 红包
        /// </summary>
        [Description("红包")]
        RedPack = 3
    }

}
