using LY.Report.Core.Common.Configs;
using LY.Report.Core.Service.Product.Abnormals;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Timer;
using Microsoft.Extensions.Hosting;
using System;
using static LY.Rport.Core.Service.LytechWebService.lytechWebServiceSoapClient;

namespace LY.Report.Core.Helper.TimerJob
{
    /// <summary>
    /// 需在ConfigureServices注册services.AddHostedService&lt;AbnormalWarnTimerJobDingDGroupDayDetails&gt;();
    /// </summary>
    public class AbnormalWarnTimerJobDingDGroupDayDetails: TimerJobHelper
    {
        //今晚23点59分
        static DateTime dt = Convert.ToDateTime(string.Format("{0}-{1}-{2} {3}:{4}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,"23","59"));
        static private TimeSpan ts1 = new TimeSpan(dt.Ticks);
        static private TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
        static TimeSpan ts = ts1.Subtract(ts2).Duration();
        //群组钉钉消息触发器 触发时间，间隔，执行者
        public AbnormalWarnTimerJobDingDGroupDayDetails(IProductAbnormalService productAbnormalService, IHostEnvironment env) : base(ts, TimeSpan.FromMinutes(1440), new AbnormalWarnJobDingDGroupDayDetailsExcutor(productAbnormalService, env))
        {

        }
    }

    #region 每天异常消息汇总钉钉群组推送
    public class AbnormalWarnJobDingDGroupDayDetailsExcutor : IJobExecutor
    {
        private readonly IProductAbnormalService _productAbnormalService;
        private readonly AppConfig _appConfig;

        public readonly LogHelper _logger = new LogHelper("AbnormalWarnTimerJobDingDGroupDayDetails");
        public AbnormalWarnJobDingDGroupDayDetailsExcutor(IProductAbnormalService deliveryCarTypeService, IHostEnvironment env)
        {
            _productAbnormalService = deliveryCarTypeService;
            _appConfig = ConfigHelper.Get<AppConfig>("appconfig", env.EnvironmentName) ?? new AppConfig();
        }

        public void StartJob()
        {
            _logger.Info("执行每日异常汇总钉钉群组消息发送任务");
            try
            {
                #region 发送钉钉
                Rport.Core.Service.LytechWebService.lytechWebServiceSoapClient client = new Rport.Core.Service.LytechWebService.lytechWebServiceSoapClient(EndpointConfiguration.lytechWebServiceSoap);
                //var strRes = client.sendGroupDingTalkLinkAsync("chat7d52a322d17c6b0f9985e79512a88249", "http://192.168.160.248:8091/ReportPage/UploadSuccessMornShift.html", "每日异常汇总报告", "查看详情：http://192.168.160.248:8091/ReportPage/UploadSuccessMornShift.html");
                var strRes = client.sendGroupDingTalkLinkAsync("chat7d52a322d17c6b0f9985e79512a88249", "http://10.0.8.59:8075/Main/PerDayAbnormalReport", "每日异常汇总报告(" + DateTime.Now.ToString("yyyy-MM-dd")+")", "查看详情：http://10.0.8.59:8075/Main/PerDayAbnormalReport");
                _logger.Info("每日异常汇总钉钉群组消息发送返回结果:" + strRes);
                #endregion
            }
            catch (Exception e)
            {
                _logger.Info("执行每日异常汇总钉钉群组消息发送任务:错误:" + e.Message);
            }
        }

        public void StopJob()
        {
            _logger.Info("系统终止任务");
        }
    }
    #endregion
}
