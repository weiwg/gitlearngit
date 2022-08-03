using LY.Report.Core.Common.Configs;
using LY.Report.Core.Model.Product.Enum;
using LY.Report.Core.Service.Product.Abnormals;
using LY.Report.Core.Service.Product.Abnormals.Input;
using LY.Report.Core.Service.Product.Abnormals.Output;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Timer;
using LY.Report.Core.Util.Tool;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections;
using static LY.Rport.Core.Service.LytechWebService.lytechWebServiceSoapClient;

namespace LY.Report.Core.Helper.TimerJob
{
    /// <summary>
    /// 需在ConfigureServices注册services.AddHostedService&lt;AbnormalWarnTimerJobDingDingGroup&gt;();
    /// </summary>
    public class AbnormalWarnTimerJobDingDingGroup : TimerJobHelper
    {
        static private readonly AppConfig _appConfig = ConfigHelper.Get<AppConfig>("appconfig", "") ?? new AppConfig();
        //群组钉钉消息触发器 触发时间，间隔，执行者
        public AbnormalWarnTimerJobDingDingGroup(IProductAbnormalService productAbnormalService, IHostEnvironment env) : base(TimeSpan.Zero, TimeSpan.FromMinutes(_appConfig.ABDDGroupInfoTimeSpan), new AbnormalWarnJobDingDingGroupExcutor(productAbnormalService, env))
        {

        }

    }

    #region 发送钉钉群组消息
    public class AbnormalWarnJobDingDingGroupExcutor : IJobExecutor
    {
        private readonly IProductAbnormalService _productAbnormalService;
        private readonly AppConfig _appConfig;

        public readonly LogHelper _logger = new LogHelper("AbnormalWarnTimerJobDingDingGroup");
        public AbnormalWarnJobDingDingGroupExcutor(IProductAbnormalService deliveryCarTypeService, IHostEnvironment env)
        {
            _productAbnormalService = deliveryCarTypeService;
            _appConfig = ConfigHelper.Get<AppConfig>("appconfig", env.EnvironmentName) ?? new AppConfig();
        }

        public async void StartJob()
        {
            _logger.Info("执行钉钉群组消息发送任务");
            try
            {
                //IDeliveryCarTypeService deliveryCarTypeService = HttpService.GetService<IDeliveryCarTypeService>();
                var res = _productAbnormalService.GetUnHandleAbnListAsync(new ProductAbnormalGetInput { AbnomalStatus = AbnormalStatus.Unhandled }).Result;
                if (res != null && res.Success)
                {
                    var abnormalGetOutputs = res.GetDataList<ProductAbnormalListOutput>();

                    ArrayList jobNoList = new ArrayList();
                    foreach (var item in abnormalGetOutputs)
                    {
                        var apList = _productAbnormalService.GetAbnormalPerson("", item.ResponDepart).Result;
                        var apo = apList.GetDataList<ProductAbnormalPersonListOutput>();

                        #region 在群组中发送钉钉信息
                        _logger.Info("执行钉钉群组消息发送任务:count:" + apo.Count);

                        #region 发送钉钉
                        Rport.Core.Service.LytechWebService.lytechWebServiceSoapClient client2 = new Rport.Core.Service.LytechWebService.lytechWebServiceSoapClient(EndpointConfiguration.lytechWebServiceSoap);
                        string strMsg2 = $"{EnumHelper.GetDescription(item.Type)}警告\n\n" +
                            $"线别：{item.ProjectNo} {item.LineName}\n\n" +
                            $"工序站点：{item.FProcess}\n\n" +
                            $"异常类型：{EnumHelper.GetDescription(item.ItemType)}\n\n" +
                            $"异常单据号：{item.AbnormalNo}\n\n" +
                            $"创建人：{item.CreateUser}\n\n" +
                            $"开始时间：{item.BeginTime}\n\n" +
                            $"指定责任人：{item.ResponName}\n\n" +
                            $"异常描述：{item.Description}\n\n" +
                            $"异常存在时间：{(DateTime.Now - item.BeginTime).TotalMinutes.ToString("0.0")}分钟,请跟进处理！\n\n" +
                            $"异常处理链接：{_appConfig.AdminReportUrl}";

                        var strRes = await client2.sendGroupDingTalkAsync("chat7d52a322d17c6b0f9985e79512a88249", $"{ EnumHelper.GetDescription(item.Type)}警告", strMsg2);
                        //返回值 {"errcode":0,"errmsg":"ok","messageId":"dfb4ed75ed253be68ba0534272dcc7a0","invalidparty":"","invaliduser":""}
                        _logger.Info("钉钉群组消息发送返回结果:" + strRes);
                        #endregion
                        #endregion

                    }
                }
            }
            catch (Exception e)
            {
                _logger.Info("执行钉钉群组消息发送任务:错误:" + e.Message);
            }
        }

        public void StopJob()
        {
            _logger.Info("系统终止任务");
        }
    }
    #endregion
}
