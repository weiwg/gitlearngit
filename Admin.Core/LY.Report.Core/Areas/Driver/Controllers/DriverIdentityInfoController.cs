using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.Driver.IdentityInfo;
using EonUp.Delivery.Core.Service.Driver.IdentityInfo.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Driver.Controllers
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
        /// 查询单条
        /// </summary>
        /// <param name="driverId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string driverId)
        {
            return await _driverIdentityInfoService.GetOneAsync(driverId);
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

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<DriverIdentityInfoGetInput> model)
        {
            return await _driverIdentityInfoService.GetPageListAsync(model);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(DriverIdentityInfoUpdateInput input)
        {
            return await _driverIdentityInfoService.UpdateEntityAsync(input);
        }
    }
}
