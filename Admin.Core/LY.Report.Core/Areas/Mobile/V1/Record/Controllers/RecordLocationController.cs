using System.Threading.Tasks;
using LY.Report.Core.Attributes;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Record.Location;
using LY.Report.Core.Service.Record.Location.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Mobile.V1.Record.Controllers
{
    /// <summary>
    /// 定位记录
    /// </summary>
    public class RecordLocationController : BaseAreaController
    {
        private readonly IRecordLocationService _recordLocationService;

        public RecordLocationController(IRecordLocationService recordLocationService)
        {
            _recordLocationService = recordLocationService;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [NoOperationLog]
        public async Task<IResponseOutput> Add(RecordLocationAddInput input)
        {
            return await _recordLocationService.AddAsync(input);
        }
    }
}
