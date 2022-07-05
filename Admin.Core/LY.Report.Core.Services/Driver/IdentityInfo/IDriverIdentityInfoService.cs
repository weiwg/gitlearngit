using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Driver.IdentityInfo.Input;

namespace LY.Report.Core.Service.Driver.IdentityInfo
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IDriverIdentityInfoService : IBaseService, IAddService<DriverIdentityInfoAddInput>, IUpdateEntityService<DriverIdentityInfoUpdateInput>, IGetService<DriverIdentityInfoGetInput>, ISoftDeleteFullService<DriverIdentityInfoDeleteInput>
    {
        /// <summary>
        /// 检查是否注册司机,并返回司机信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> CheckRegisterAsync(DriverIdentityInfoGetInput input);

        /// <summary>
        /// 获取当前用户认证信息
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetCurrUserIdentityInfoAsync();
    }
}
