using EonUp.Delivery.Core.Attributes;
using EonUp.Delivery.Core.CacheRepository;
using EonUp.Delivery.Core.CacheRepository.Enum;
using EonUp.Delivery.Core.Common.Auth;
using EonUp.Delivery.Core.Common.Cache;
using EonUp.Delivery.Core.Common.Captcha;
using EonUp.Delivery.Core.Common.Captcha.Dtos;
using EonUp.Delivery.Core.Common.Consts;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.EupApiUtil.Ua;
using EonUp.Delivery.Core.Service.Record.Login;
using EonUp.Delivery.Core.Service.Record.Login.Input;
using EonUp.Delivery.Core.Service.System.Cache;
using EonUp.Delivery.Core.Service.User.Account;
using EonUp.Delivery.Core.Service.User.Account.Input;
using EonUp.Delivery.Core.Service.User.Account.Output;
using EonUp.Delivery.Core.Util.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;

namespace EonUp.Delivery.Core.Areas.User.Controllers
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

        /// <summary>
        /// 获取图片验证码
        /// </summary>
        /// <param name="lastKey">上次验证码键</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoOperationLog]
        [NoPermission]
        public async Task<IResponseOutput> GetImgVerifyCode(string lastKey)
        {
            return await _accountService.GetImgVerifyCodeAsync(lastKey);
        }

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
        public async Task<IResponseOutput> Login(EupUaLoginInput input)
        {
            var sw = new Stopwatch();
            sw.Start();
            IResponseOutput res;
            if (GlobalConfig.EupLoginModel != EupLoginModel.Local)
            {
                res = await _accountService.EupUaLoginAsync(input);
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

        /// <summary>
        /// 获取验证数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoOperationLog]
        [NoPermission]
        [EnableCors(TenantConsts.AllowAnyPolicyName)]
        public async Task<IResponseOutput> GetCaptcha()
        {
            var data = await _captcha.GetAsync();
            return ResponseOutput.Data(data);
        }

        /// <summary>
        /// 检查验证数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [NoOperationLog]
        [NoPermission]
        [EnableCors(TenantConsts.AllowAnyPolicyName)]
        public async Task<IResponseOutput> CheckCaptcha([FromQuery] CaptchaInput input)
        {
            var result = await _captcha.CheckAsync(input);
            return ResponseOutput.Result(result);
        }
    }
}
