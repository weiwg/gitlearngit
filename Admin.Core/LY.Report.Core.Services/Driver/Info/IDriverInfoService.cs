using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Driver.Info.Input;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Driver.Info
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IDriverInfoService: IBaseService, IUpdateService<DriverInfoUpdateInput>, IGetService<DriverInfoGetInput>, ISoftDeleteFullService<DriverInfoDeleteInput>
    {
        /// <summary>
        /// ���̳ǵ���
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateStoreBindAsync(DriverInfoUpdateStoreBindInput input);

        /// <summary>
        /// ����̳ǵ���
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateStoreUnboundAsync(DriverInfoUpdateStoreUnboundInput input);

        /// <summary>
        /// ������̨����̳ǵ���
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateSysStoreUnboundAsync(DriverInfoUpdateStoreUnboundInput input);
        /// <summary>
        /// ע��˾��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateSysCancelDriverAsync(DriverInfoDeleteInput input);

        /// <summary>
        /// �û�ע��˾��
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> UpdateCancelDriverAsync();



    }
}
