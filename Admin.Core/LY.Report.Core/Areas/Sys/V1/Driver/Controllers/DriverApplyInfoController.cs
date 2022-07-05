using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LY.Report.Core.Areas.Sys.V1.Driver.Controllers
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
        /// 查询单条
        /// </summary>
        /// <param name="applyId">申请Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string applyId)
        {
            return await _driverApplyInfoService.GetOneAsync(applyId);
        }
        
        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<DriverApplyInfoGetInput> model)
        {
            return await _driverApplyInfoService.GetPageListAsync(model);
        }
        #endregion

        #region 修改
        /// <summary>
        /// 司机信息审核
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdateApplyApproval(DriverApplyInfoUpdateApplyApprovalInput input)
        {
            return await _driverApplyInfoService.UpdateApplyApprovalAsync(input);
        }
        #endregion

        #endregion

    }
}
