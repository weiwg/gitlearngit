using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.User.Account.Input;
using LY.Report.Core.Service.User.Account.Output;
using LY.Report.Core.Service.User.Info.Input;

namespace LY.Report.Core.Service.User.Account
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IAccountService: IBaseService
    {
        #region 验证码

        /// <summary>
        /// 获取用户手机/邮箱验证码
        /// </summary>
        /// <param name="action">phone/email</param>
        /// <returns></returns>
        Task<IResponseOutput> GetUserVerifyCodeAsync(string action);

        /// <summary>
        /// 获取手机/邮箱验证码
        /// </summary>
        /// <param name="account">手机/邮箱</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetVerifyCodeAsync(string account, string userId = "");

        /// <summary>
        /// 校验账户验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> CheckUserVerifyCodeAsync(VerifyCodeInput input);

        /// <summary>
        /// 获取图片验证码
        /// </summary>
        /// <param name="lastKey"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetImgVerifyCodeAsync(string lastKey);
        #endregion

        #region 账号校验
        /// <summary>
        /// 判断登录名,手机号,邮箱是否存在
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetCheckUserAccount(CheckAcountInput input);
        #endregion

        #region 登录
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isAutoLogin"></param>
        /// <returns></returns>
        Task<IResponseOutput> LoginAsync(LYUaLoginInput input, bool isAutoLogin = false);

        /// <summary>
        /// 统一登录验证
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> LYUaLoginAsync(LYUaLoginInput input);
        #endregion

        #region 注册

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> Register(UserInfoAddInput input);
        #endregion

        #region token
        /// <summary>
        /// 获得token
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        IResponseOutput GetToken(LoginOutput output);

        /// <summary>
        /// 刷新Token
        /// 以旧换新
        /// </summary>
        /// <param name="oldToken"></param>
        /// <returns></returns>
        Task<IResponseOutput> RefreshToken(string oldToken);
        #endregion
    }
}
