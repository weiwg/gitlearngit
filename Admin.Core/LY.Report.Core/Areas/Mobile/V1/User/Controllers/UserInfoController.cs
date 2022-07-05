using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.User.Info;
using LY.Report.Core.Service.User.Info.Input;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LY.Report.Core.Areas.Mobile.V1.User.Controllers
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
        /// 修改头像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdatePortrait(UserInfoUpdatePortraitInput input)
        {
            return await _userInfoService.UpdatePortraitAsync(input);
        }

        /// <summary>
        /// 解绑微信
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UntieWeChat()
        {
            return await _userInfoService.UpdateUserUntieWeChatAsync();
        }

        /// <summary>
        /// 修改手机/邮箱
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdateAccount(UserInfoUpdateAccountInput input)
        {
            return await _userInfoService.UpdateAccountAsync(input);
        }

        #endregion
    }
}
