using System.Threading.Tasks;
using EonUp.Delivery.Core.Attributes;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.Record.Location;
using EonUp.Delivery.Core.Service.Record.Location.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Record.Controllers
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
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<RecordLocationGetInput> model)
        {
            return await _recordLocationService.GetPageListAsync(model);
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
