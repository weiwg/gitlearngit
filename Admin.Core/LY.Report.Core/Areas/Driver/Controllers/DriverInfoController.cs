using System.Threading.Tasks;
using EonUp.Delivery.Core.Attributes;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.Driver.IdentityInfo;
using EonUp.Delivery.Core.Service.Driver.IdentityInfo.Input;
using EonUp.Delivery.Core.Service.Driver.Info;
using EonUp.Delivery.Core.Service.Driver.Info.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Driver.Controllers
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
        /// 检查是否注册司机,并返回司机信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [AllowEupApi]
        [HttpGet]
        public async Task<IResponseOutput> CheckRegister(string userId)
        {
            if (userId.IsNull())
            {
                return ResponseOutput.NotOk("参数错误");
            }

            var res = await _driverIdentityInfoService.CheckRegisterAsync(new DriverIdentityInfoGetInput {UserId = userId});
            if (!res.Success)
            {
                return ResponseOutput.Ok("司机未注册");
            }
            return res;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<DriverInfoGetInput> model)
        {
            return await _driverInfoService.GetPageListAsync(model);
        }

        ///// <summary>
        ///// 新增
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<IResponseOutput> Add(DriverInfoAddInput input)
        //{
        //    return await _driverInfoService.AddAsync(input);
        //}

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
        /// 用户注销司机
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdateCancelDriver()
        {
            return await _driverInfoService.UpdateCancelDriverAsync();
        }
        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpDelete]
        //public async Task<IResponseOutput> SoftDelete(string id)
        //{
        //    return await _driverInfoService.SoftDeleteAsync(id);
        //}

        ///// <summary>
        ///// 批量删除
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //[HttpPut]
        //public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        //{
        //    return await _driverInfoService.BatchSoftDeleteAsync(ids);
        //}

        /// <summary>
        /// 绑定商城店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowEupApi]
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
        [AllowEupApi]
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
        [AllowEupApi]
        [HttpPost]
        public async Task<IResponseOutput> UpdateSysStoreUnbound(DriverInfoUpdateStoreUnboundInput input)
        {
            return await _driverInfoService.UpdateSysStoreUnboundAsync(input);
        }
    }
}
