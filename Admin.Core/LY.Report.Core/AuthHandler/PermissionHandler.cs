using LY.Report.Core.Common.Auth;
using LY.Report.Core.Enums;
using LY.Report.Core.Repository.Auth.UserRole;
using LY.Report.Core.Service.User.Info;
using LY.Report.Core.Util.Tool;
using System.Linq;
using System.Threading.Tasks;
using BaseApiVersion = LY.Report.Core.Common.BaseModel.Enum.ApiVersion;

namespace LY.Report.Core.Auth
{
    /// <summary>
    /// 权限处理
    /// </summary>
    public class PermissionHandler: IPermissionHandler
    {
        private readonly IUserInfoService _userService;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUser _user;

        public PermissionHandler(IUserInfoService userService, IUserRoleRepository userRoleRepository, IUser user)
        {
            _userService = userService;
            _userRoleRepository = userRoleRepository;
            _user = user;
        }

        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="api">接口路径</param>
        /// <param name="httpMethod">http请求方法</param>
        /// <returns></returns>
        public async Task<bool> ValidateAsync(string api, string httpMethod)
        {
            //校验当前用户是不是特殊的角色，如果是则返回true,反之false
            if (await IsNoCheckPermission())
            {
                return true;
            }
            string strVersion = EnumHelper.GetDescription(_user.ApiVersion);//获取版本号字符串
            var permissions = await _userService.GetPermissionsAsync(strVersion);

            var valid = permissions.Any(m => 
                m.Path.IsNotNull() && m.Path.EqualsIgnoreCase($"/{api}")
                && m.HttpMethods.IsNotNull() && m.HttpMethods.Split(',').Any(n => n.IsNotNull() && n.EqualsIgnoreCase(httpMethod))
            );

            return valid;
        }

        /// <summary>
        /// 特殊权限免校验
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsNoCheckPermission()
        {
            var permissions = await _userService.GetSpecialPermissionsAsync();

            var valid = permissions.Any(m =>
                m.UserId.IsNotNull() && m.UserId.Contains(_user.UserId)
            );
            return valid;
        }
    }
}
