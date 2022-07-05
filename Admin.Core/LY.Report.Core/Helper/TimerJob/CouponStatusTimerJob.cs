using LY.Report.Core.Common.Configs;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Timer;
using System;

namespace LY.Report.Core.Helper.TimerJob
{
    /// <summary>
    /// 需在ConfigureServices注册services.AddHostedService&lt;CouponStatusTimerJob&gt;();
    /// </summary>
    public class CouponStatusTimerJob : TimerJobHelper
    {
        #if DEBUG
        private static TimerConfig timerConfig = ConfigHelper.Get<TimerConfig>("timerconfig", "Development") ?? new TimerConfig();
        #else
        private static TimerConfig timerConfig = ConfigHelper.Get<TimerConfig>("timerconfig") ?? new TimerConfig();
        #endif

        /// <summary>
        /// 触发器 触发时间，间隔，执行者
        /// </summary>
        public CouponStatusTimerJob(ISalesCouponService salesCouponService, IUserCouponService userCouponService) : base(TimeSpan.FromMinutes(timerConfig.CouponStatus.DueTime), TimeSpan.FromMinutes(timerConfig.CouponStatus.PeriodTime), new CouponStatusJobExcutor(timerConfig, salesCouponService, userCouponService))
        {
        }
    }

    public class CouponStatusJobExcutor : IJobExecutor
    {
        private readonly TimerConfig _timerConfig;
        private readonly ISalesCouponService _salesCouponService;
        private readonly IUserCouponService _userCouponService;
        private readonly LogHelper _logger = new LogHelper("CouponStatusTimerJob");

        /// <summary>
        ///  运行状态
        /// </summary>
        public static bool IsRunning { get; protected set; }

        public CouponStatusJobExcutor(TimerConfig timerConfig, ISalesCouponService salesCouponService, IUserCouponService userCouponService)
        {
            _timerConfig = timerConfig;
            _salesCouponService = salesCouponService;
            _userCouponService = userCouponService;
        }

        public void StartJob()
        {
            if (IsRunning || !_timerConfig.RedPackStatus.IsOpen)
            {
                return;
            }

            IsRunning = true;
            _logger.Info("执行任务_定时处理优惠券状态");
            try
            {
                var res = _salesCouponService.CheckSysCouponStatusTimerJob().Result;
                if (!res.Success)
                {
                    _logger.Error("处理系统优惠券状态错误:", res.Msg);
                }

                res = _userCouponService.CheckUserCouponStatusTimerJob().Result;
                if (!res.Success)
                {
                    _logger.Error("处理用户优惠券配置状态错误:", res.Msg);
                }

                IsRunning = false;
            }
            catch (Exception e)
            {
                IsRunning = false;
                _logger.Error("定时处理优惠券状态错误:" + e.Message);
            }

            _logger.Info("执行任务_定时处理优惠券状态_结束");
        }

        public void StopJob()
        {
            IsRunning = false;
            _logger.Info("系统终止任务_定时处理优惠券状态");
        }
    }
}
