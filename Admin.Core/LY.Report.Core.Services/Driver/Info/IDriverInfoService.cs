using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Driver.Info.Input;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Driver.Info
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IDriverInfoService: IBaseService, IUpdateService<DriverInfoUpdateInput>, IGetService<DriverInfoGetInput>, ISoftDeleteFullService<DriverInfoDeleteInput>
    {
        /// <summary>
        /// 绑定商城店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateStoreBindAsync(DriverInfoUpdateStoreBindInput input);

        /// <summary>
        /// 解绑商城店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateStoreUnboundAsync(DriverInfoUpdateStoreUnboundInput input);

        /// <summary>
        /// 物流后台解绑商城店铺
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateSysStoreUnboundAsync(DriverInfoUpdateStoreUnboundInput input);
        /// <summary>
        /// 注销司机
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateSysCancelDriverAsync(DriverInfoDeleteInput input);

        /// <summary>
        /// 用户注销司机
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> UpdateCancelDriverAsync();



    }
}
