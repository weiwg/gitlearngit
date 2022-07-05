namespace LY.Report.Core.Common.Auth
{
    /// <summary>
    /// Claim属性
    /// </summary>
    public static class ClaimAttributes
    {
        /// <summary>
        /// 身份Id
        /// </summary>
        public const string SessionId = "sid";

        /// <summary>
        /// 用户Id
        /// </summary>
        public const string UserId = "uid";

        /// <summary>
        /// OpenId
        /// </summary>
        public const string OpenId = "oid";

        /// <summary>
        /// WeChatOpenId
        /// </summary>
        public const string WeChatOpenId = "wxoid";

        /// <summary>
        /// 司机Id
        /// </summary>
        public const string DriverId = "did";
        
        /// <summary>
        /// 认证授权用户Id
        /// </summary>
        public const string IdentityServerUserId = "sub";

        /// <summary>
        /// 用户名
        /// </summary>
        public const string UserName = "un";

        /// <summary>
        /// 昵称
        /// </summary>
        public const string UserNickName = "unn";

        /// <summary>
        /// 刷新有效期
        /// </summary>
        public const string RefreshExpires = "re";

        /// <summary>
        /// 租户Id
        /// </summary>
        public const string TenantId = "ti";

        /// <summary>
        /// 租户类型
        /// </summary>
        public const string TenantType = "tt";

        /// <summary>
        /// 数据隔离
        /// </summary>
        public const string DataIsolationType = "dit";
    }
}
