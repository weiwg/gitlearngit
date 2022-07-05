using EonUp.Delivery.Core.Attributes;
using EonUp.Delivery.Core.Common.Input;
using EonUp.Delivery.Core.Common.Output;
using EonUp.Delivery.Core.Service.User.Info;
using EonUp.Delivery.Core.Service.User.Info.Input;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EonUp.Delivery.Core.Areas.User.Controllers
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

        #region 新增
        ///// <summary>
        ///// 新增
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<IResponseOutput> Add(UserInfoAddInput input)
        //{
        //    return await _userInfoService.AddAsync(input);
        //}
        #endregion

        #region 查询
        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> Get(string id)
        {
            return await _userInfoService.GetOneAsync(id);
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetPage(PageInput<UserInfoGetInput> model)
        {
            return await _userInfoService.GetPageListAsync(model);
        }

        /// <summary>
        /// 获取用户下拉
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> GetSelectList(UserInfoGetSelectInput model)
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

        /// <summary>
        /// 获取当前用户资金余额
        /// </summary>
        /// <returns></returns>
        [HttpGet] 
        public async Task<IResponseOutput> GetCurrUserBalance()
        {
            return await _userInfoService.GetCurrUserBalanceAsync();
        }

        /// <summary>
        /// 获取当前用户资金余额红包优惠券
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IResponseOutput> GetCurrUserFullFund(bool getCouponList)
        {
            return await _userInfoService.GetCurrUserFullFundAsync(getCouponList);
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
        /// 修改支付密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> UpdatePayPassword(UserInfoUpdatePayPasswordInput input)
        {
            return await _userInfoService.UpdatePayPasswordAsync(input);
        }

        /// <summary>
        /// 重置支付密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> ResetPayPassword(UserInfoResetPayPasswordInput input)
        {
            return await _userInfoService.ResetPayPasswordAsync(input);
        }

        /// <summary>
        /// 后台重置支付密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> ResetSysPayPassword(UserInfoResetPaySysPasswordInput input)
        {
            return await _userInfoService.ResetSysPayPasswordAsync(input);
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

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IResponseOutput> SoftDelete(string id)
        {
            return await _userInfoService.SoftDeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IResponseOutput> BatchSoftDelete(string[] ids)
        {
            return await _userInfoService.BatchSoftDeleteAsync(ids);
        }
        #endregion
    }
}
