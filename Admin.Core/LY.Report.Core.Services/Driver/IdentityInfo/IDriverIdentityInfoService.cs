using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Driver.IdentityInfo.Input;

namespace LY.Report.Core.Service.Driver.IdentityInfo
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IDriverIdentityInfoService : IBaseService, IAddService<DriverIdentityInfoAddInput>, IUpdateEntityService<DriverIdentityInfoUpdateInput>, IGetService<DriverIdentityInfoGetInput>, ISoftDeleteFullService<DriverIdentityInfoDeleteInput>
    {
        /// <summary>
        /// ����Ƿ�ע��˾��,������˾����Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> CheckRegisterAsync(DriverIdentityInfoGetInput input);

        /// <summary>
        /// ��ȡ��ǰ�û���֤��Ϣ
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetCurrUserIdentityInfoAsync();
    }
}
