using LY.Report.Core.Attributes;
using LY.Report.Core.CacheRepository;
using LY.Report.Core.CacheRepository.Enum;
using LY.Report.Core.Common.Auth;
using LY.Report.Core.Common.Cache;
using LY.Report.Core.Common.Captcha;
using LY.Report.Core.Common.Captcha.Dtos;
using LY.Report.Core.Common.Consts;
using LY.Report.Core.Common.Output;
using LY.Report.Core.LYApiUtil.Ua;
using LY.Report.Core.Service.Record.Login;
using LY.Report.Core.Service.Record.Login.Input;
using LY.Report.Core.Service.System.Cache;
using LY.Report.Core.Service.User.Account;
using LY.Report.Core.Service.User.Account.Input;
using LY.Report.Core.Service.User.Account.Output;
using LY.Report.Core.Util.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;

namespace LY.Report.Core.Areas.Mobile.V1.User.Controllers
{
    /// <summary>
    /// 账号管理
    /// </summary>
    public class AccountController : BaseAreaController
    {
        private readonly IAccountService _accountService;
        private readonly IRecordLoginService _recordLoginService;
        private readonly IUser _user;
        private readonly ICaptcha _captcha;
        private readonly ICacheService _cacheService;
        private readonly LogHelper _logger = new LogHelper("AccountController");

        public AccountController(IAccountService accountService, IRecordLoginService recordLoginService, IUser user, ICaptcha captcha, ICacheService cacheService)
        {
            _accountService = accountService;
            _recordLoginService = recordLoginService;
            _user = user;
            _captcha = captcha;
            _cacheService = cacheService;
        }

        #region 验证码
        /// <summary>
        /// 获取用户手机/邮箱验证码
        /// </summary>
        /// <param name="action">phone/email</param>
        /// <returns></returns>
        [HttpGet]
        [NoOperationLog]
        public async Task<IResponseOutput> GetUserVerifyCode(string action)
        {
            return await _accountService.GetUserVerifyCodeAsync(action);
        }

        /// <summary>
        /// 校验用户账号验证码
        /// </summary>
        /// <param name="input">验证码</param>
        /// <returns></returns>
        [HttpPost]
        [NoOperationLog]
        public async Task<IResponseOutput> CheckUserVerifyCode(VerifyCodeInput input)
        {
            return await _accountService.CheckUserVerifyCodeAsync(input);
        }

        /// <summary>
        /// 获取手机/邮箱验证码
        /// </summary>
        /// <param name="account">手机/邮箱</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoOperationLog]
        [NoPermission]
        public async Task<IResponseOutput> GetVerifyCode(string account)
        {
            return await _accountService.GetVerifyCodeAsync(account);
        }

        ///// <summary>
        ///// 获取图片验证码
        ///// </summary>
        ///// <param name="lastKey">上次验证码键</param>
        ///// <returns></returns>
        //[HttpGet]
        //[AllowAnonymous]
        //[NoOperationLog]
        //[NoPermission]
        //public async Task<IResponseOutput> GetImgVerifyCode(string lastKey)
        //{
        //    return await _accountService.GetImgVerifyCodeAsync(lastKey);
        //}

        #endregion

        /// <summary>
        /// 用户登录
        /// 根据登录信息生成Token
        /// </summary>
        /// <param name="input">登录信息</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [NoOperationLog]
        [NoPermission]
        public async Task<IResponseOutput> Login(LYUaLoginInput input)
        {
            var sw = new Stopwatch();
            sw.Start();
            IResponseOutput res;
            if (GlobalConfig.LYLoginModel != LYLoginModel.Local)
            {
                res = await _accountService.LYUaLoginAsync(input);
            }
            else
            {
                res = await _accountService.LoginAsync(input);
            }
            sw.Stop();

            #region 添加登录日志
            LoginOutput user = new LoginOutput();

            var recordLoginAddInput = new RecordLoginAddInput()
            {
                ElapsedMilliseconds = sw.ElapsedMilliseconds,
                Status = res.Success,
                Msg = res.Msg
            };

            if (res.Success)
            {
                user = res.GetData<LoginOutput>();
                recordLoginAddInput.CreateUserId = user.UserId;
                recordLoginAddInput.UserName = user.UserName;
            }

            await _recordLoginService.AddAsync(recordLoginAddInput);
            #endregion

            if (!res.Success)
            {
                return res;
            }

            return _accountService.GetToken(user);
        }

        /// <summary>
        /// 刷新Token
        /// 以旧换新
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoPermission]
        public async Task<IResponseOutput> Refresh([BindRequired] string token)
        {
            return await _accountService.RefreshToken(token);
        }

        /// <summary>
        /// 统一登录退出
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoPermission]
        public IResponseOutput Logout(string returnUrl)
        {
            _cacheService.ClearAsync(string.Format(CacheKey.UserPermissions, _user.UserId, _user.ApiVersion));
            return ResponseOutput.Data(UaApiHelper.GetUrlLogout(HttpUtility.UrlEncode(returnUrl), _user?.SessionId));
        }

        ///// <summary>
        ///// 获取验证数据
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[AllowAnonymous]
        //[NoOperationLog]
        //[NoPermission]
        //[EnableCors(TenantConsts.AllowAnyPolicyName)]
        //public async Task<IResponseOutput> GetCaptcha()
        //{
        //    var data = await _captcha.GetAsync();
        //    return ResponseOutput.Data(data);
        //}

        ///// <summary>
        ///// 检查验证数据
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[AllowAnonymous]
        //[NoOperationLog]
        //[NoPermission]
        //[EnableCors(TenantConsts.AllowAnyPolicyName)]
        //public async Task<IResponseOutput> CheckCaptcha([FromQuery] CaptchaInput input)
        //{
        //    var result = await _captcha.CheckAsync(input);
        //    return ResponseOutput.Result(result);
        //}
    }
}
