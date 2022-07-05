using LY.Report.Core.Common.Input;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.User.Info;
using LY.Report.Core.Service.User.Info.Input;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LY.Report.Core.Areas.Sys.V1.User.Controllers
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoController : BaseAreaController
    {
        private readonly IUserInfoService _userInfoService;

        public UserInfoController(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

        #region 查询

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetPage([FromQuery] PageInput<UserInfoGetInput> model)
        {
            return await _userInfoService.GetPageListAsync(model);
        }

        /// <summary>
        /// 获取用户下拉
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetSelectList([FromQuery] UserInfoGetSelectInput model)
        {
            return await _userInfoService.GetSelectListAsync(model);
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetCurrUserInfo()
        {
            return await _userInfoService.GetCurrUserInfoAsync();
        }

        #endregion

        #region 修改
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> Update(UserInfoUpdateInput input)
        {
            return await _userInfoService.UpdateAsync(input);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdatePassword(UserInfoUpdatePasswordInput input)
        {
            return await _userInfoService.UpdatePasswordAsync(input);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> ResetSysPassword(UserInfoResetPasswordInput input)
        {
            return await _userInfoService.ResetPasswordAsync(input);
        }
        /// <summary>
        /// 重置邮箱
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> ResetEmail(UserInfoResetEmailInput input)
        {
            return await _userInfoService.ResetEmailAsync(input);
        }

        /// <summary>
        /// 重置手机
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> ResetPhone(UserInfoResetPhoneInput input)
        {
            return await _userInfoService.ResetPhoneAsync(input);
        }

        #endregion

    }
}
