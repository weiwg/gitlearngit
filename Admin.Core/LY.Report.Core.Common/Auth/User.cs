using System;
using LY.Report.Core.Common.BaseModel.Enum;
using Microsoft.AspNetCore.Http;

namespace LY.Report.Core.Common.Auth
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class User : IUser
    {
        private readonly IHttpContextAccessor _accessor;

        public User(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        /// <summary>
        /// 身份Id
        /// </summary>
        public string SessionId => GetClaimVal(ClaimAttributes.SessionId);

        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual string UserId => GetClaimVal(ClaimAttributes.UserId);

        /// <summary>
        /// 用户OpenId
        /// </summary>
        public virtual string OpenId => GetClaimVal(ClaimAttributes.OpenId);

        /// <summary>
        /// 当前浏览器微信OpenId,不一定是登录用户OpenId
        /// </summary>
        public virtual string WeChatOpenId => GetClaimVal(ClaimAttributes.WeChatOpenId);

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName => GetClaimVal(ClaimAttributes.UserName);

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName => GetClaimVal(ClaimAttributes.UserNickName);

        /// <summary>
        /// 司机Id
        /// </summary>
        public virtual string DriverId => GetClaimVal(ClaimAttributes.DriverId);

        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual string TenantId => GetClaimVal(ClaimAttributes.TenantId);

        /// <summary>
        /// 接口版本号
        /// </summary>
        public ApiVersion ApiVersion { get; set; } = ApiVersion.M_V1;

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProName { get; set; } = "HEB";

        /// <summary>
        /// 租户类型
        /// </summary>
        public virtual TenantType? TenantType
        {
            get
            {
                var tenantType = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.TenantType);
                if (tenantType != null && tenantType.Value.IsNotNull())
                {
                    return (TenantType)Enum.Parse(typeof(TenantType), tenantType.Value, true);
                }
                return null;
            }
        }


        /// <summary>
        /// 数据隔离
        /// </summary>
        public virtual DataIsolationType? DataIsolationType
        {
            get
            {
                var dataIsolationType = _accessor?.HttpContext?.User?.FindFirst(ClaimAttributes.DataIsolationType);
                if (dataIsolationType != null && dataIsolationType.Value.IsNotNull())
                {
                    return (DataIsolationType)Enum.Parse(typeof(DataIsolationType), dataIsolationType.Value, true);
                }
                return null;
            }
        }

        /// <summary>
        /// 获取Claim值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string GetClaimVal(string type)
        {
            var item = _accessor?.HttpContext?.User?.FindFirst(type);
            if (item != null && item.Value.IsNotNull())
            {
                return item.Value;
            }
            return "";
        }
    }
}
