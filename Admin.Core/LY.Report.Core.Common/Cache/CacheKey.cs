
using System.ComponentModel;

namespace LY.Report.Core.Common.Cache
{
    /// <summary>
    /// 缓存键
    /// </summary>
    public static class CacheKey
    {
        /// <summary>
        /// 验证码 report:verify:code:guid
        /// </summary>
        [Description("验证码")]
        public const string VerifyCodeKey = "report:verify:code:{0}";

        /// <summary>
        /// 验证码频率 report:verify:rate:phone/email
        /// </summary>
        [Description("验证码频率")]
        public const string VerifyRateKey = "report:verify:rate:{0}";

        /// <summary>
        /// 验证码凭据 report:verify:token:userId
        /// </summary>
        [Description("验证码凭据")]
        public const string VerifyTokenKey = "report:verify:token:{0}";

        /// <summary>
        /// 密码加密 report:password:encrypt:guid
        /// </summary>
        [Description("密码加密")]
        public const string PassWordEncryptKey = "report:password:encrypt:{0}";

        /// <summary>
        /// 用户权限 report:user:用户主键:permissions
        /// </summary>
        [Description("用户权限")]
        public const string UserPermissions = "report:user:{0}:{1}:permissions";

        /// <summary>
        /// 用户信息 report:user:info:用户主键
        /// </summary>
        [Description("用户信息")]
        public const string UserInfo = "report:user:info:{0}";

        /// <summary>
        /// 用户Token report:user:token:用户主键
        /// </summary>
        [Description("用户Token")]
        public const string UserToken = "report:user:token:{0}";

        /// <summary>
        /// 租户信息 report:tenant:info:租户主键
        /// </summary>
        [Description("租户信息")]
        public const string TenantInfo = "report:tenant:info:{0}";

        /// <summary>
        /// 特殊权限 report:permissions:role:用户id
        /// </summary>
        [Description("特殊权限")]
        public const string NoCheckPermission = "report:permissions:role:{0}:{1}";
    }
}
