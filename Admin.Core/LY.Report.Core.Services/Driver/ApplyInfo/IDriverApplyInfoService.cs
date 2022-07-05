using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.Driver.ApplyInfo.Input;

namespace LY.Report.Core.Service.Driver.ApplyInfo
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IDriverApplyInfoService : IBaseService, IAddService<DriverApplyInfoAddInput>, IUpdateService<DriverApplyInfoUpdateInput>, IGetService<DriverApplyInfoGetInput>, ISoftDeleteFullService<DriverApplyInfoDeleteInput>
    {
        #region ����
        /// <summary>
        /// ��ȡ��ǰ�û�������Ϣ
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetCurrUserApplyInfoAsync();

        /// <summary>
        /// �����ύ��ǰ�û�������Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> ReSubmitCurrUserApplyInfoAsync(DriverApplyInfoUpdateInput input);
        /// <summary>
        /// ˾����Ϣ���
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateApplyApprovalAsync(DriverApplyInfoUpdateApplyApprovalInput input);
        #endregion

        #region ˾����Ϣ
        /// <summary>
        /// �����޸�˾����Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> ApplyUpdateDriverAsync(DriverApplyInfoAddInput input);

        #endregion
    }
}
