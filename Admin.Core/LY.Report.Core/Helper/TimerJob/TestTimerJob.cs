using LY.Report.Core.Model.BaseEnum;
using LY.Report.Core.Service.Demo.Test;
using LY.Report.Core.Service.Demo.Test.Input;
using LY.Report.Core.Service.Demo.Test.Output;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Timer;
using System;

namespace LY.Report.Core.Helper.TimerJob
{
    /// <summary>
    /// 需在ConfigureServices注册services.AddHostedService&lt;TestTimerJob&gt;();
    /// </summary>
    public class TestTimerJob : TimerJobHelper
    {
        /// <summary>
        /// 触发器 触发时间，间隔，执行者
        /// </summary>
        public TestTimerJob(IDemoTestService deliveryCarTypeService) : base(TimeSpan.Zero, TimeSpan.FromMinutes(5), new TestJobExcutor(deliveryCarTypeService))
        {
        }
    }

    public class TestJobExcutor : IJobExecutor
    {
        private readonly LogHelper _logger = new LogHelper("TestTimerJob");
        private readonly IDemoTestService _demoTestService;
        public TestJobExcutor(IDemoTestService deliveryCarTypeService)
        {
            _demoTestService = deliveryCarTypeService;
        }

        public void StartJob()
        {
            _logger.Info("执行任务");
            try
            {
                //IDeliveryCarTypeService deliveryCarTypeService = HttpService.GetService<IDeliveryCarTypeService>();
                var res = _demoTestService.GetListAsync(new DemoTestGetInput { IsActive = IsActive.Yes }).Result;
                if (res != null && res.Success)
                {
                    var deliveryCarType = res.GetDataList<DemoTestListOutput>();

                    _logger.Info("执行任务:count:" + deliveryCarType.Count);
                }
            }
            catch (Exception e)
            {
                _logger.Info("执行任务:错误:" + e.Message);
            }
        }

        public void StopJob()
        {
            _logger.Info("系统终止任务");
        }
    }
}
