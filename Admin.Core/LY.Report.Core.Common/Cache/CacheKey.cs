
using System.ComponentModel;

namespace LY.Report.Core.Common.Cache
{
    /// <summary>
    /// 缓存键
    /// </summary>
    public static class CacheKey
    {
        /// <summary>
        /// 验证码 delivery:verify:code:guid
        /// </summary>
        [Description("验证码")]
        public const string VerifyCodeKey = "delivery:verify:code:{0}";

        /// <summary>
        /// 验证码频率 delivery:verify:rate:phone/email
        /// </summary>
        [Description("验证码频率")]
        public const string VerifyRateKey = "delivery:verify:rate:{0}";

        /// <summary>
        /// 验证码凭据 delivery:verify:token:userId
        /// </summary>
        [Description("验证码凭据")]
        public const string VerifyTokenKey = "delivery:verify:token:{0}";

        /// <summary>
        /// 密码加密 delivery:password:encrypt:guid
        /// </summary>
        [Description("密码加密")]
        public const string PassWordEncryptKey = "delivery:password:encrypt:{0}";

        /// <summary>
        /// 用户权限 delivery:user:用户主键:permissions
        /// </summary>
        [Description("用户权限")]
        public const string UserPermissions = "delivery:user:{0}:{1}:permissions";

        /// <summary>
        /// 用户信息 delivery:user:info:用户主键
        /// </summary>
        [Description("用户信息")]
        public const string UserInfo = "delivery:user:info:{0}";

        /// <summary>
        /// 用户Token delivery:user:token:用户主键
        /// </summary>
        [Description("用户Token")]
        public const string UserToken = "delivery:user:token:{0}";

        /// <summary>
        /// 租户信息 delivery:tenant:info:租户主键
        /// </summary>
        [Description("租户信息")]
        public const string TenantInfo = "delivery:tenant:info:{0}";

        /// <summary>
        /// 特殊权限 delivery:permissions:role:用户id
        /// </summary>
        [Description("特殊权限")]
        public const string NoCheckPermission = "delivery:permissions:role:{0}:{1}";
    }
}
