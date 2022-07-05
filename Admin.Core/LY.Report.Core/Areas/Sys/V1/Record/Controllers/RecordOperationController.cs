using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Record.Operation;
using LY.Report.Core.Service.Record.Operation.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Sys.V1.Record.Controllers
{
    /// <summary>
    /// 操作记录
    /// </summary>
    public class RecordOperationController : BaseAreaController
    {
        private readonly IRecordOperationService _recordOperationService;

        public RecordOperationController(IRecordOperationService recordOperationService)
        {
            _recordOperationService = recordOperationService;
        }
        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<RecordOperationGetInput> model)
        {
            return await _recordOperationService.GetPageListAsync(model);
        }
    }
}
