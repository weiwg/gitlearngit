using System;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.Service.Order.Info;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Timer;

namespace LY.Report.Core.Helper.TimerJob
{
    /// <summary>
    /// 需在ConfigureServices注册services.AddHostedService&lt;OrderStatusTimerJob&gt;();
    /// </summary>
    public class OrderStatusTimerJob : TimerJobHelper
    {
        #if DEBUG
        private static TimerConfig timerConfig = ConfigHelper.Get<TimerConfig>("timerconfig", "Development") ?? new TimerConfig();
        #else
        private static TimerConfig timerConfig = ConfigHelper.Get<TimerConfig>("timerconfig") ?? new TimerConfig();
        #endif

        /// <summary>
        /// 触发器 触发时间，间隔，执行者
        /// </summary>
        public OrderStatusTimerJob(IOrderInfoService orderInfoService) : base(TimeSpan.FromMinutes(timerConfig.OrderStatus.DueTime), TimeSpan.FromMinutes(timerConfig.OrderStatus.PeriodTime), new OrderStatusJobExcutor(timerConfig, orderInfoService))
        {
        }
    }

    public class OrderStatusJobExcutor : IJobExecutor
    {
        private readonly TimerConfig _timerConfig;
        private readonly IOrderInfoService _orderInfoService;
        private readonly LogHelper _logger = new LogHelper("OrderStatusTimerJob");

        /// <summary>
        ///  运行状态
        /// </summary>
        public static bool IsRunning { get; protected set; }

        public OrderStatusJobExcutor(TimerConfig timerConfig, IOrderInfoService orderInfoService)
        {
            _timerConfig = timerConfig;
            _orderInfoService = orderInfoService;
        }

        public void StartJob()
        {
            if (IsRunning || !_timerConfig.OrderStatus.IsOpen)
            {
                return;
            }

            IsRunning = true;
            _logger.Info("执行任务_定时查询订单状态");
            try
            {
                var res = _orderInfoService.CheckWaitingOrderStatusTimerJob().Result;
                if (!res.Success)
                {
                    _logger.Error("查询待接单订单状态错误:", res.Msg);
                }

                res = _orderInfoService.CheckUserConfirmStatusTimerJob().Result;
                if (!res.Success)
                {
                    _logger.Error("查询检查用户确认送达状态错误:", res.Msg);
                }

                IsRunning = false;
            }
            catch (Exception e)
            {
                IsRunning = false;
                _logger.Error("定时查询订单状态错误:" + e.Message);
            }
            _logger.Info("执行任务_定时查询订单状态_结束");
        }

        public void StopJob()
        {
            IsRunning = false;
            _logger.Info("系统终止任务_定时查询订单状态");
        }
    }
}
