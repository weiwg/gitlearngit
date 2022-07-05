

using LY.Report.Core.Service.User.Coupon.Output;
using System.Collections.Generic;

namespace LY.Report.Core.Service.User.Info.Output
{
    /// <summary>
    /// 用户资金余额
    /// </summary>
    public class UserFundGetFullOutput : UserFundGetOutput
    {
        /// <summary>
        /// 红包
        /// </summary>
        public decimal RedPack { get; set; } = 0;

        /// <summary>
        /// 优惠券列表
        /// </summary>
        public List<UserCouponListOutput> CouponList { get; set; } = new List<UserCouponListOutput>();

        /// <summary>
        /// 优惠券
        /// </summary>
        public long Coupon { get; set; } = 0;

    }
}
