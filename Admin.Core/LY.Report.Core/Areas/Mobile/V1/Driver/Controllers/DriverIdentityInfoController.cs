using System.Threading.Tasks;
using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Driver.IdentityInfo;
using LY.Report.Core.Service.Driver.IdentityInfo.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Mobile.V1.Driver.Controllers
{
    /// <summary>
    /// 司机认证信息管理
    /// </summary>
    public class DriverIdentityInfoController : BaseAreaController
    {
        private readonly IDriverIdentityInfoService _driverIdentityInfoService;

        public DriverIdentityInfoController(IDriverIdentityInfoService driverIdentityInfoService)
        {
            _driverIdentityInfoService = driverIdentityInfoService;
        }

        /// <summary>
        /// 获取当前用户认证信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetCurrUser()
        {
            return await _driverIdentityInfoService.GetCurrUserIdentityInfoAsync();
        }
    }
}
