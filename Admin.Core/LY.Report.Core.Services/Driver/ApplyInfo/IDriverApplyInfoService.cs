using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Driver.ApplyInfo.Input;

namespace LY.Report.Core.Service.Driver.ApplyInfo
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IDriverApplyInfoService : IBaseService, IAddService<DriverApplyInfoAddInput>, IUpdateService<DriverApplyInfoUpdateInput>, IGetService<DriverApplyInfoGetInput>, ISoftDeleteFullService<DriverApplyInfoDeleteInput>
    {
        #region 申请
        /// <summary>
        /// 获取当前用户申请信息
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetCurrUserApplyInfoAsync();

        /// <summary>
        /// 重新提交当前用户申请信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> ReSubmitCurrUserApplyInfoAsync(DriverApplyInfoUpdateInput input);
        /// <summary>
        /// 司机信息审核
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateApplyApprovalAsync(DriverApplyInfoUpdateApplyApprovalInput input);
        #endregion

        #region 司机信息
        /// <summary>
        /// 申请修改司机信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> ApplyUpdateDriverAsync(DriverApplyInfoAddInput input);

        #endregion
    }
}
