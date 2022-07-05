using System.Collections.Generic;
using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.User.Info.Input;
using LY.Report.Core.Service.User.Info.Output;

namespace LY.Report.Core.Service.User.Info
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IUserInfoService : IBaseService, IAddService<UserInfoAddInput>, IUpdateService<UserInfoUpdateInput>, IGetService<UserInfoGetInput>, ISoftDeleteFullService<UserInfoDeleteInput>
    {

        /// <summary>
        /// ��ȡ��¼�û���Ϣ
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetCurrUserInfoAsync();

        /// <summary>
        /// �޸��û�����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdatePasswordAsync(UserInfoUpdatePasswordInput input);

        /// <summary>
        /// �����û�����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> ResetPasswordAsync(UserInfoResetPasswordInput input);

        /// <summary>
        /// �޸��û�ͷ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdatePortraitAsync(UserInfoUpdatePortraitInput input);

        /// <summary>
        /// ���΢��
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> UpdateUserUntieWeChatAsync();

        /// <summary>
        /// �޸��ֻ�/����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateAccountAsync(UserInfoUpdateAccountInput input);

        /// <summary>
        /// �����ֻ�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> ResetPhoneAsync(UserInfoResetPhoneInput input);

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> ResetEmailAsync(UserInfoResetEmailInput input);


        /// <summary>
        /// ��ȡ�û�����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetSelectListAsync(UserInfoGetSelectInput input);

        /// <summary>
        /// ��ȡȨ��
        /// </summary>
        /// <param name="apiVersion">�汾��</param>
        /// <returns></returns>
        Task<IList<UserPermissionsOutput>> GetPermissionsAsync(string apiVersion);

        /// <summary>
        /// ��ȡ����Ȩ��
        /// </summary>
        /// <returns></returns>
        Task<IList<UserIsNoCheckPermissionsOutput>> GetSpecialPermissionsAsync();

    }
}
