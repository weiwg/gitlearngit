using LY.Report.Core.Common.BaseModel.Enum;

namespace LY.Report.Core.Common.Auth
{
    /// <summary>
    /// 用户信息接口
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// 身份Id
        /// </summary>
        string SessionId { get; }

        /// <summary>
        /// 用户Id
        /// </summary>
        string UserId { get; }

        /// <summary>
        /// 用户OpenId
        /// </summary>
        string OpenId { get; }

        /// <summary>
        /// 当前浏览器微信OpenId,不一定是登录用户OpenId
        /// </summary>
        string WeChatOpenId { get; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// 昵称
        /// </summary>
        string NickName { get; }

        /// <summary>
        /// 司机Id
        /// </summary>
        string DriverId { get; }

        /// <summary>
        /// 租户Id
        /// </summary>
        string TenantId { get; }

        /// <summary>
        /// 租户类型
        /// </summary>
        TenantType? TenantType { get; }

        /// <summary>
        /// 数据隔离
        /// </summary>
        DataIsolationType? DataIsolationType { get; }

        /// <summary>
        /// 接口版本号
        /// </summary>
        ApiVersion ApiVersion { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        string ProName { get; set; }
    }
}
