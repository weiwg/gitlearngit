using Microsoft.AspNetCore.Http;
using LY.Report.Core.Common.Helpers;

namespace LY.Report.Core.Common.Auth
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserIdentiyServer : User
    {
        private readonly IHttpContextAccessor _accessor;

        public UserIdentiyServer(IHttpContextAccessor accessor) : base(accessor)
        {
            _accessor = accessor;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public override string UserId
        {
            get
            {
                var id = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.IdentityServerUserId);
                if (id != null && id.Value.IsNotNull())
                {
                    return id.Value;
                }
                return "";
            }
        }
    }
}
