using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Record.Operation;
using LY.Report.Core.Service.Record.Operation.Input;
using LY.Report.Core.Util.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace LY.Report.Core.Helper
{
    /// <summary>
    /// 操作日志处理
    /// </summary>
    public class LogHandler : ILogHandler
    {
        private readonly ILogger _logger;
        private readonly LogHelper _logger1;
        private readonly ApiHelper _apiHelper;
        private readonly IRecordOperationService _operationLogService;

        public LogHandler(
            ILogger<LogHandler> logger,
            ApiHelper apiHelper, 
            IRecordOperationService operationLogService
        )
        {
            _logger = logger;
            _logger1 = new LogHelper("LogHandler");
            _apiHelper = apiHelper;
            _operationLogService = operationLogService;
        }

        public async Task LogAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sw = new Stopwatch();
            sw.Start();
            var actionExecutedContext = await next();
            sw.Stop();

            //操作参数
            //var args = JsonConvert.SerializeObject(context.ActionArguments);
            //操作结果
            //var result = JsonConvert.SerializeObject(actionResult?.Value);

            try
            {
                var input = new RecordOperationAddInput
                {
                    ApiMethod = context.HttpContext.Request.Method.ToLower(),
                    ApiPath = context.ActionDescriptor.AttributeRouteInfo.Template.ToLower(),
                    ElapsedMilliseconds = sw.ElapsedMilliseconds
                };

                if (actionExecutedContext.Result is ObjectResult result && result.Value is IResponseOutput res)
                {
                    input.Status = res.Success;
                    input.Msg = res.Msg;
                }

                input.ApiLabel = _apiHelper.GetApis().FirstOrDefault(a => a.Path == input.ApiPath)?.Label;
                
                await _operationLogService.AddAsync(input);
            }
            catch (Exception ex)
            {
                _logger.LogError("操作日志插入异常：{@ex}", ex);
                _logger1.Error($"操作日志插入异常：{ex}");
            }
        }
    }
}
