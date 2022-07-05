namespace LY.Report.Core.Common.Configs
{
    /// <summary>
    /// 定时配置
    /// </summary>
    public class TimerConfig
    {
        /// <summary>
        /// 支付状态
        /// </summary>
        public TimerJobTime PayStatus { get; set; } = new TimerJobTime();

        /// <summary>
        /// 订单状态
        /// </summary>
        public TimerJobTime OrderStatus { get; set; } = new TimerJobTime();

        /// <summary>
        /// 红包状态
        /// </summary>
        public TimerJobTime RedPackStatus { get; set; } = new TimerJobTime();

        /// <summary>
        /// 优惠券状态
        /// </summary>
        public TimerJobTime CouponStatus { get; set; } = new TimerJobTime();
    }

    /// <summary>
    /// 定时配置
    /// </summary>
    public class TimerJobTime
    {
        /// <summary>
        /// 是否开启
        /// </summary>
        public bool IsOpen { get; set; } = false;
        /// <summary>
        /// 开始执行时间(分钟)
        /// </summary>
        public double DueTime { get; set; } = 1;
        /// <summary>
        /// 间隔时间(分钟)
        /// </summary>
        public double PeriodTime { get; set; } = 10;
    }
}
