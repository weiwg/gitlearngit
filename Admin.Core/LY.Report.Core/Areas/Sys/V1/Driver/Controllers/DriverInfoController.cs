using System.Threading.Tasks;
using LY.Report.Core.Attributes;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Driver.Info;
using LY.Report.Core.Service.Driver.Info.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Sys.V1.Driver.Controllers
{
    /// <summary>
    /// 司机管理
    /// </summary>
    public class DriverInfoController : BaseAreaController
    {
        private readonly IDriverInfoService _driverInfoService;

        public DriverInfoController(IDriverInfoService driverInfoService)
        {
            _driverInfoService = driverInfoService;
        }

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string id)
        {
            return await _driverInfoService.GetOneAsync(id);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<DriverInfoGetInput> model)
        {
            return await _driverInfoService.GetPageListAsync(model);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(DriverInfoUpdateInput input)
        {
            return await _driverInfoService.UpdateAsync(input);
        }
        /// <summary>
        /// 注销司机
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdateSysCancelDriver(DriverInfoDeleteInput input)
        {
            return await _driverInfoService.UpdateSysCancelDriverAsync(input);
        }

        /// <summary>
        /// 系统解绑商城店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowEupApi]
        [HttpPost]
        public async Task<IResponseOutput> UpdateSysStoreUnbound(DriverInfoUpdateStoreUnboundInput input)
        {
            return await _driverInfoService.UpdateSysStoreUnboundAsync(input);
        }
    }
}
