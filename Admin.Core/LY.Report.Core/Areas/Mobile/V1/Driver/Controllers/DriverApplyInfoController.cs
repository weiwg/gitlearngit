using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Driver.ApplyInfo;
using LY.Report.Core.Service.Driver.ApplyInfo.Input;
using Microsoft.AspNetCore.Mvc;

namespace LY.Report.Core.Areas.Mobile.V1.Driver.Controllers
{
    /// <summary>
    /// 司机申请管理
    /// </summary>
    public class DriverApplyInfoController : BaseAreaController
    {
        private readonly IDriverApplyInfoService _driverApplyInfoService;

        public DriverApplyInfoController(IDriverApplyInfoService driverApplyInfoService)
        {
            _driverApplyInfoService = driverApplyInfoService;
        }

        #region 申请

        #region 查询

        /// <summary>
        /// 获取当前用户申请信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetCurrUser()
        {
            return await _driverApplyInfoService.GetCurrUserApplyInfoAsync();
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Add(DriverApplyInfoAddInput input)
        {
            return await _driverApplyInfoService.AddAsync(input);
        }
        #endregion

        #region 修改

        /// <summary>
        /// 重新提交当前用户申请信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> ReSubmitCurrUser(DriverApplyInfoUpdateInput input)
        {
            return await _driverApplyInfoService.ReSubmitCurrUserApplyInfoAsync(input); 
        }

        #endregion


        #endregion

        #region 司机信息

        /// <summary>
        /// 申请修改司机信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> ApplyUpdateDriver(DriverApplyInfoAddInput input)
        {
            return await _driverApplyInfoService.ApplyUpdateDriverAsync(input);
        }

        #endregion

    }
}
