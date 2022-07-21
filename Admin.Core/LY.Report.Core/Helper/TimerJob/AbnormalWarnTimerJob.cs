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
    /// 需在ConfigureServices注册services.AddHostedService&lt;AbnormalWarnTimerJob&gt;();
    /// </summary>
    public class AbnormalWarnTimerJob: TimerJobHelper
    {
        /// <summary>
        /// 触发器 触发时间，间隔，执行者
        /// </summary>
        public AbnormalWarnTimerJob(IProductAbnormalService productAbnormalService, IHostEnvironment env) : base(TimeSpan.Zero, TimeSpan.FromMinutes(5), new AbnormalWarnJobExcutor(productAbnormalService, env))
        {
        }
      
    }

    public class AbnormalWarnJobExcutor : IJobExecutor
    {
        private readonly LogHelper _logger = new LogHelper("AbnormalWarnTimerJob");
        private readonly IProductAbnormalService _productAbnormalService;
        private readonly AppConfig _appConfig;
        public AbnormalWarnJobExcutor(IProductAbnormalService deliveryCarTypeService, IHostEnvironment env)
        {
            _productAbnormalService = deliveryCarTypeService;
            _appConfig = ConfigHelper.Get<AppConfig>("appconfig", env.EnvironmentName) ?? new AppConfig();
        }

        public void StartJob()
        {
            _logger.Info("执行任务");
            try
            {
                //IDeliveryCarTypeService deliveryCarTypeService = HttpService.GetService<IDeliveryCarTypeService>();
                var res = _productAbnormalService.GetUnHandleAbnListAsync(new ProductAbnormalGetInput { AbnomalStatus = AbnormalStatus.Unhandled }).Result;
                if (res != null && res.Success)
                {
                    var abnormalGetOutputs = res.GetDataList<ProductAbnormalListOutput>();

                    ArrayList jobNoList = new ArrayList();
                    foreach(var item in abnormalGetOutputs)
                    {
                        var apList = _productAbnormalService.GetAbnormalPerson("", item.ResponDepart).Result;
                        var apo = apList.GetDataList<ProductAbnormalPersonListOutput>();
                        foreach (var apEntity in apo)
                        {
                            jobNoList.Add(apEntity.JobNo);
                        }
                        var arrJobNoNos = jobNoList.ToArray();
                        var strJobNos = string.Join('|', arrJobNoNos);
                        _logger.Info("执行任务:count:" + apo.Count);

                        #region 发送钉钉
                        Rport.Core.Service.LytechWebService.lytechWebServiceSoapClient client = new Rport.Core.Service.LytechWebService.lytechWebServiceSoapClient(EndpointConfiguration.lytechWebServiceSoap);

                        //string userId = "10237422|10166850|10013699";
                        string userId = strJobNos;
                        //string userId = "10555656";
                        string strMsg = "您好\n" +
                                                    $"{EnumHelper.GetDescription(item.Type)}警告\n" +
                                                    $"工序站点：{item.FProcess}\n" +
                                                    $"异常类型：{EnumHelper.GetDescription(item.ItemType)}\n" +
                                                    $"异常单据号：{item.AbnormalNo}\n" +
                                                    $"创建人：{item.CreateUser}\n" +
                                                    $"开始时间：{item.BeginTime}\n" +
                                                    $"指定责任人：{item.ResponName}\n" +
                                                    $"异常描述：{item.Description}\n" +
                                                    $"存在时间：{(DateTime.Now - item.BeginTime).TotalMinutes.ToString("0.00")}分钟,请跟进处理！\n" +
                                                    $"异常处理连接：{_appConfig.AdminReportUrl}";
                        if (userId.Length > 0)
                        {
                            //var res = client.setDingDTalkAsync(userId, strMsg);
                            client.setDingDTalkAsync(userId, strMsg);
                            //res.Wait(15);
                        }
                        //返回值 {"errcode":0,"errmsg":"ok","messageId":"dfb4ed75ed253be68ba0534272dcc7a0","invalidparty":"","invaliduser":""}
                        #endregion
                    }
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
