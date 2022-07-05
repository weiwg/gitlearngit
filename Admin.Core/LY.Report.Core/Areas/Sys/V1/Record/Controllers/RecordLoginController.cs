using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Record.Login;
using LY.Report.Core.Service.Record.Login.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Sys.V1.Record.Controllers
{
    /// <summary>
    /// 登录记录
    /// </summary>
    public class RecordLoginController : BaseAreaController
    {
        private readonly IRecordLoginService _recordLoginService;

        public RecordLoginController(IRecordLoginService recordLoginService)
        {
            _recordLoginService = recordLoginService;
        }
        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<RecordLoginGetInput> model)
        {
            return await _recordLoginService.GetPageListAsync(model);
        }
    }
}
