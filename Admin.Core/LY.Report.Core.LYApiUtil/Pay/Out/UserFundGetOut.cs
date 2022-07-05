

namespace LY.Report.Core.LYApiUtil.Pay.Out
{
    /// <summary>
    /// 用户资金余额
    /// </summary>
    public class UserFundGetOut
    {
        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; } = 0;

        /// <summary>
        /// 冻结余额
        /// </summary>
        public decimal FrozenBalance { get; set; } = 0;

        /// <summary>
        /// 不可提现余额
        /// </summary>
        public decimal NoWithdrawBalance { get; set; } = 0;

    }
}
