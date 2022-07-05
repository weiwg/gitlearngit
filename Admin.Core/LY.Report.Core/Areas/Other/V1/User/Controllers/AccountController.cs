using LY.Report.Core.Attributes;
using LY.Report.Core.Common.Captcha;
using LY.Report.Core.Common.Captcha.Dtos;
using LY.Report.Core.Common.Consts;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.User.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LY.Report.Core.Areas.Other.V1.User.Controllers
{
    /// <summary>
    /// 账号管理
    /// </summary>
    public class AccountController : BaseAreaController
    {
        private readonly IAccountService _accountService;
        private readonly ICaptcha _captcha;

        public AccountController(IAccountService accountService, ICaptcha captcha)
        {
            _accountService = accountService;
            _captcha = captcha;
        }

        #region 验证码
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
