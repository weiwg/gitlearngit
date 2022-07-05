using System.Threading.Tasks;
using LY.Report.Core.Attributes;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Driver.IdentityInfo;
using LY.Report.Core.Service.Driver.IdentityInfo.Input;
using LY.Report.Core.Service.Driver.Info;
using LY.Report.Core.Service.Driver.Info.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Open.V1.Driver.Controllers
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
        /// 检查是否注册司机,并返回司机信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> CheckRegister(string userId)
        {
            if (userId.IsNull())
            {
                return ResponseOutput.NotOk("参数错误");
            }

            var res = await _driverIdentityInfoService.GetOneAsync(new DriverIdentityInfoGetInput {UserId = userId});
            if (!res.Success)
            {
                return ResponseOutput.Ok("司机未注册");
            }
            return res;
        }

        /// <summary>
        /// 绑定商城店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> UpdateStoreBind(DriverInfoUpdateStoreBindInput input)
        {
            return await _driverInfoService.UpdateStoreBindAsync(input);
        }
        /// <summary>
        /// 解绑商城店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> UpdateStoreUnbound(DriverInfoUpdateStoreUnboundInput input)
        {
            return await _driverInfoService.UpdateStoreUnboundAsync(input);
        }

        /// <summary>
        /// 系统解绑商城店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> UpdateSysStoreUnbound(DriverInfoUpdateStoreUnboundInput input)
        {
            return await _driverInfoService.UpdateSysStoreUnboundAsync(input);
        }
    }
}
