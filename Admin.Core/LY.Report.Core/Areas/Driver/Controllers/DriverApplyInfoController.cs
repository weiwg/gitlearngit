using System.Threading.Tasks;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.Driver.ApplyInfo;
using EonUp.Delivery.Core.Service.Driver.ApplyInfo.Input;
using Microsoft.AspNetCore.Mvc;

namespace EonUp.Delivery.Core.Areas.Driver.Controllers
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
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<DriverApplyInfoGetInput> model)
        {
            return await _driverApplyInfoService.GetPageListAsync(model);
        }

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
        ///// <summary>
        ///// 修改
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPut]
        //public async Task<IResponseOutput> Update(DriverApplyInfoUpdateInput input)
        //{
        //    return await _driverApplyInfoService.UpdateAsync(input);
        //}

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

        #region 删除
        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpDelete]
        //public async Task<IResponseOutput> SoftDelete(string id)
        //{
        //    return await _driverApplyInfoService.SoftDeleteAsync(id);
        //}

        ///// <summary>
        ///// 批量删除
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //[HttpPut]
        //public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        //{
        //    return await _driverApplyInfoService.BatchSoftDeleteAsync(ids);
        //}
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
