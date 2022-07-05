using System;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.Service.Pay.Income;
using LY.Report.Core.Service.Pay.Refund;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Timer;

namespace LY.Report.Core.Helper.TimerJob
{
    /// <summary>
    /// 需在ConfigureServices注册services.AddHostedService&lt;PayStatusTimerJob&gt;();
    /// </summary>
    public class PayStatusTimerJob : TimerJobHelper
    {
        #if DEBUG
        private static TimerConfig timerConfig = ConfigHelper.Get<TimerConfig>("timerconfig", "Development") ?? new TimerConfig();
        #else
        private static TimerConfig timerConfig = ConfigHelper.Get<TimerConfig>("timerconfig") ?? new TimerConfig();
        #endif

        /// <summary>
        /// 触发器 触发时间，间隔，执行者
        /// </summary>
        public PayStatusTimerJob(IPayIncomeService payIncomeService, IPayRefundService payRefundService) : base(TimeSpan.FromMinutes(timerConfig.PayStatus.DueTime), TimeSpan.FromMinutes(timerConfig.PayStatus.PeriodTime), new PayStatusJobExcutor(timerConfig, payIncomeService, payRefundService))
        {
        }
    }

    public class PayStatusJobExcutor : IJobExecutor
    {
        private readonly TimerConfig _timerConfig;
        private readonly IPayIncomeService _payIncomeService;
        private readonly IPayRefundService _payRefundService;
        private readonly LogHelper _logger = new LogHelper("PayStatusTimerJob");

        /// <summary>
        ///  运行状态
        /// </summary>
        public static bool IsRunning { get; protected set; }

        public PayStatusJobExcutor(TimerConfig timerConfig, IPayIncomeService payIncomeService, IPayRefundService payRefundService)
        {
            _timerConfig = timerConfig;
            _payIncomeService = payIncomeService;
            _payRefundService = payRefundService;
        }

        public void StartJob()
        {
            if (IsRunning || !_timerConfig.PayStatus.IsOpen)
            {
                return;
            }

            IsRunning = true;
            _logger.Info("执行任务_定时查询支付状态");
            try
            {
                var res = _payIncomeService.CheckPayStatusTimerJob().Result;
                if (!res.Success)
                {
                    _logger.Error("处理支付状态错误:", res.Msg);
                }

                res = _payRefundService.TradeRefundTimerJob().Result;
                if (!res.Success)
                {
                    _logger.Error("处理订单退款:", res.Msg);
                }
                IsRunning = false;
            }
            catch (Exception e)
            {
                IsRunning = false;
                _logger.Error("定时查询支付状态错误:" + e.Message);
            }
            _logger.Info("执行任务_定时查询支付状态_结束");
        }

        public void StopJob()
        {
            IsRunning = false;
            _logger.Info("系统终止任务_定时查询支付状态");
        }
    }
}
