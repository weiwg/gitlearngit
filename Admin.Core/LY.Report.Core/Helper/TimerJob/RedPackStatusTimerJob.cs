using System;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.Service.Sales.RedPack;
using LY.Report.Core.Service.User.RedPack;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Timer;

namespace LY.Report.Core.Helper.TimerJob
{
    /// <summary>
    /// 需在ConfigureServices注册services.AddHostedService&lt;RedPackStatusTimerJob&gt;();
    /// </summary>
    public class RedPackStatusTimerJob : TimerJobHelper
    {
        #if DEBUG
        private static TimerConfig timerConfig = ConfigHelper.Get<TimerConfig>("timerconfig", "Development") ?? new TimerConfig();
        #else
        private static TimerConfig timerConfig = ConfigHelper.Get<TimerConfig>("timerconfig") ?? new TimerConfig();
        #endif

        /// <summary>
        /// 触发器 触发时间，间隔，执行者
        /// </summary>
        public RedPackStatusTimerJob(IUserRedPackService  userRedPackService, ISalesRedPackService salesRedPackService) : base(TimeSpan.FromMinutes(timerConfig.RedPackStatus.DueTime), TimeSpan.FromMinutes(timerConfig.RedPackStatus.PeriodTime), new RedPackStatusJobExcutor(timerConfig, userRedPackService, salesRedPackService))
        {
        }
    }

    public class RedPackStatusJobExcutor : IJobExecutor
    {
        private readonly TimerConfig _timerConfig;
        private readonly ISalesRedPackService _salesRedPackService;
        private readonly IUserRedPackService _userRedPackService;
        private readonly LogHelper _logger = new LogHelper("RedPackStatusTimerJob");
        /// <summary>
        ///  运行状态
        /// </summary>
        public static bool IsRunning { get; protected set; }

        public RedPackStatusJobExcutor(TimerConfig timerConfig, IUserRedPackService userRedPackService, ISalesRedPackService salesRedPackService)
        {
            _timerConfig = timerConfig;
            _userRedPackService = userRedPackService;
            _salesRedPackService = salesRedPackService;
        }

        public void StartJob()
        {
            if (IsRunning || !_timerConfig.RedPackStatus.IsOpen)
            {
                return;
            }

            IsRunning = true;
            _logger.Info("执行任务_定时处理红包状态");
            try
            {
                var res = _salesRedPackService.CheckSysRedPackStatusTimerJob().Result;
                if (!res.Success)
                {
                    _logger.Error("处理系统红包配置状态错误:", res.Msg);
                }

                res = _userRedPackService.CheckUserRedPackStatusTimerJob().Result;
                if (!res.Success)
                {
                    _logger.Error("处理用户红包状态错误:", res.Msg);
                }

                IsRunning = false;
            }
            catch (Exception e)
            {
                IsRunning = false;
                _logger.Error("定时处理红包状态错误:" + e.Message);
            }

            _logger.Info("执行任务_定时处理红包状态_结束");
        }

        public void StopJob()
        {
            IsRunning = false;
            _logger.Info("系统终止任务_定时处理红包状态");
        }
    }
}
