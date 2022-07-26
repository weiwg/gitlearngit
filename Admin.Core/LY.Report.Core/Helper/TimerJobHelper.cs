using System;
using System.Threading;
using System.Threading.Tasks;
using LY.Report.Core.Common.Attributes;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Timer;
using Microsoft.Extensions.Hosting;

namespace LY.Report.Core.Helper
{
    /// <summary>
    /// 定时任务帮助类
    /// </summary>
    public abstract class TimerJobHelper : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly TimeSpan _dueTime;
        private readonly TimeSpan _periodTime;
        private readonly LogHelper _logger = new LogHelper("TimerJobHelper");

        private readonly IJobExecutor _jobExcutor;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dueTime">开始执行时间</param>
        /// <param name="periodTime">间隔时间</param>
        /// <param name="jobExcutor">任务执行者</param>
        protected TimerJobHelper(TimeSpan dueTime,TimeSpan periodTime,IJobExecutor jobExcutor)
        {
            _dueTime = dueTime;
            _periodTime = periodTime;
            _jobExcutor = jobExcutor;
        }

        #region  计时器相关方法

        private void StartTimerTrigger()
        {
            if (_timer == null)
            {
                _timer = new Timer(ExcuteJob, _jobExcutor, _dueTime, _periodTime);
            }
            else
            {
                _timer.Change(_dueTime, _periodTime);
            }
        }

        private void StopTimerTrigger()
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void ExcuteJob(object obj)
        {
            try
            {
                var excutor = obj as IJobExecutor;
                excutor?.StartJob();
            }
            catch (Exception e)
            {
                _logger.Error($"执行任务({nameof(GetType)})时出错，信息：{e}");
            }
        }
        #endregion

        /// <summary>
        /// 系统级任务执行启动
        /// StartAsync和StopAsync 为 IHostService 系统服务接口，表示托管服务的开始和结束。
        /// </summary>
        /// <returns></returns>
        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                StartTimerTrigger();
            }
            catch (Exception e)
            {
                _logger.Error($"启动定时任务({nameof(GetType)})时出错，信息：{e}");
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 系统级任务执行关闭
        /// StartAsync和StopAsync 为 IHostService 系统服务接口，表示托管服务的开始和结束。
        /// </summary>
        /// <returns></returns>
        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _jobExcutor.StopJob();
                StopTimerTrigger();
            }
            catch (Exception e)
            {
                _logger.Error($"停止定时任务({nameof(GetType)})时出错，信息：{e}");
            }
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
