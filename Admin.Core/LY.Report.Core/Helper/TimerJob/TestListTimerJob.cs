using System;
using System.Collections.Generic;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Timer;

namespace LY.Report.Core.Helper.TimerJob
{
    /// <summary>
    /// 需在ConfigureServices注册services.AddHostedService&lt;TestListJobTrigger&gt;();
    /// </summary>
    public class TestListTimerJob : TimerJobHelper
    {
        public TestListTimerJob() : base(TimeSpan.Zero,TimeSpan.FromMinutes(10),new ListJobExcutor())
        {
        }
    }

    public class ListJobExcutor : ListJobExcutor<string>
    {
        private readonly LogHelper _logger = new LogHelper("TestListJobTrigger");

        private int _page = 0;

        protected override IList<string> GetExcuteSource()
        {
            if (_page == 0)
            {
                _page++;
                return new List<string> { "1", "2", "3" };
            }
            return null;
        }

        protected override void ExcuteItem(string item, int index)
        {
            _logger.Info(item);
        }
    }
}
