using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Driver.IdentityInfo;
using LY.Report.Core.Service.Driver.IdentityInfo.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Sys.V1.Driver.Controllers
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
