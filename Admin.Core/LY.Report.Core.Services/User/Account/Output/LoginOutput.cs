
namespace LY.Report.Core.Service.User.Account.Output
{
    public class LoginOutput
    {
        /// <summary>
        /// 身份Id
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// OpenId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string NickName { get; set; }

        private string _tenantId;
        /// <summary>
        /// 租户Id
        /// </summary>
        public string TenantId { get { return string.IsNullOrEmpty(_tenantId) ? "" : _tenantId; } set { _tenantId = value; } }

        /// <summary>
        /// 微信OpenId(当前微信浏览器的OpenId,不一定是登录用户的OpenId)
        /// </summary>
        public string WeChatOpenId { get; set; }
    }
}
