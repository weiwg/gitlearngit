using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.User.Account.Input;
using LY.Report.Core.Service.User.Account.Output;
using LY.Report.Core.Service.User.Info.Input;

namespace LY.Report.Core.Service.User.Account
{
    /// <summary>
    /// �ӿڷ���
    /// </summary>	
    public interface IAccountService: IBaseService
    {
        #region ��֤��

        /// <summary>
        /// ��ȡ�û��ֻ�/������֤��
        /// </summary>
        /// <param name="action">phone/email</param>
        /// <returns></returns>
        Task<IResponseOutput> GetUserVerifyCodeAsync(string action);

        /// <summary>
        /// ��ȡ�ֻ�/������֤��
        /// </summary>
        /// <param name="account">�ֻ�/����</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetVerifyCodeAsync(string account, string userId = "");

        /// <summary>
        /// У���˻���֤��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> CheckUserVerifyCodeAsync(VerifyCodeInput input);

        /// <summary>
        /// ��ȡͼƬ��֤��
        /// </summary>
        /// <param name="lastKey"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetImgVerifyCodeAsync(string lastKey);
        #endregion

        #region �˺�У��
        /// <summary>
        /// �жϵ�¼��,�ֻ���,�����Ƿ����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetCheckUserAccount(CheckAcountInput input);
        #endregion

        #region ��¼
        /// <summary>
        /// ��¼��֤
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isAutoLogin"></param>
        /// <returns></returns>
        Task<IResponseOutput> LoginAsync(LYUaLoginInput input, bool isAutoLogin = false);

        /// <summary>
        /// ͳһ��¼��֤
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> LYUaLoginAsync(LYUaLoginInput input);
        #endregion

        #region ע��

        /// <summary>
        /// ע��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> Register(UserInfoAddInput input);
        #endregion

        #region token
        /// <summary>
        /// ���token
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        IResponseOutput GetToken(LoginOutput output);

        /// <summary>
        /// ˢ��Token
        /// �Ծɻ���
        /// </summary>
        /// <param name="oldToken"></param>
        /// <returns></returns>
        Task<IResponseOutput> RefreshToken(string oldToken);
        #endregion
    }
}
