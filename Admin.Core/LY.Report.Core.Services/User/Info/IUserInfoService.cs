using System.Collections.Generic;
using System.Threading.Tasks;
using LY.Report.Core.Common.Output;
using LY.Report.Core.Service.Base.IService;
using LY.Report.Core.Service.User.Info.Input;
using LY.Report.Core.Service.User.Info.Output;

namespace LY.Report.Core.Service.User.Info
{
    /// <summary>
    /// 接口服务
    /// </summary>	
    public interface IUserInfoService : IBaseService, IAddService<UserInfoAddInput>, IUpdateService<UserInfoUpdateInput>, IGetService<UserInfoGetInput>, ISoftDeleteFullService<UserInfoDeleteInput>
    {

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> GetCurrUserInfoAsync();

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdatePasswordAsync(UserInfoUpdatePasswordInput input);

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> ResetPasswordAsync(UserInfoResetPasswordInput input);

        /// <summary>
        /// 修改用户头像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdatePortraitAsync(UserInfoUpdatePortraitInput input);

        /// <summary>
        /// 解绑微信
        /// </summary>
        /// <returns></returns>
        Task<IResponseOutput> UpdateUserUntieWeChatAsync();

        /// <summary>
        /// 修改手机/邮箱
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> UpdateAccountAsync(UserInfoUpdateAccountInput input);

        /// <summary>
        /// 重置手机
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> ResetPhoneAsync(UserInfoResetPhoneInput input);

        /// <summary>
        /// 重置邮箱
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> ResetEmailAsync(UserInfoResetEmailInput input);


        /// <summary>
        /// 获取用户下拉
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> GetSelectListAsync(UserInfoGetSelectInput input);

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="apiVersion">版本号</param>
        /// <returns></returns>
        Task<IList<UserPermissionsOutput>> GetPermissionsAsync(string apiVersion);

        /// <summary>
        /// 获取特殊权限
        /// </summary>
        /// <returns></returns>
        Task<IList<UserIsNoCheckPermissionsOutput>> GetSpecialPermissionsAsync();

    }
}
