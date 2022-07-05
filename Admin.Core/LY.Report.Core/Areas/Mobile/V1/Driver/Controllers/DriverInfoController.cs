using System.Threading.Tasks;
using LY.Report.Core.Attributes;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Driver.IdentityInfo;
using LY.Report.Core.Service.Driver.Info;
using LY.Report.Core.Service.Driver.Info.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Mobile.V1.Driver.Controllers
{
    /// <summary>
    /// 司机管理
    /// </summary>
    public class DriverInfoController : BaseAreaController
    {
        private readonly IDriverInfoService _driverInfoService;
        private readonly IDriverIdentityInfoService _driverIdentityInfoService;

        public DriverInfoController(IDriverInfoService driverInfoService, IDriverIdentityInfoService driverIdentityInfoService)
        {
            _driverInfoService = driverInfoService;
            _driverIdentityInfoService = driverIdentityInfoService;
        }

        /// <summary>
        /// 用户注销司机
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdateCancelDriver()
        {
            return await _driverInfoService.UpdateCancelDriverAsync();
        }
     
        /// <summary>
        /// 解绑商城店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowEupApi]
        [HttpPost]
        public async Task<IResponseOutput> UpdateStoreUnbound(DriverInfoUpdateStoreUnboundInput input)
        {
            return await _driverInfoService.UpdateStoreUnboundAsync(input);
        }

    }
}
