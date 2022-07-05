

namespace LY.Report.Core.LYApiUtil.Pay.Out
{
    /// <summary>
    /// 系统资金余额
    /// </summary>
    public class AppFundGetOut
    {
        /// <summary>
        /// 余额
        /// </summary>
        public decimal Balance { get; set; } = 0;

        /// <summary>
        /// 冻结余额
        /// </summary>
        public decimal FrozenBalance { get; set; } = 0;

    }
}
