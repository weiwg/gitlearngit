
namespace LY.Report.Core.Common.Configs
{
    /// <summary>
    /// EupAuthConfig配置
    /// </summary>
    public class LYAuthConfig
    {
        /// <summary>
        /// 否使用EUA API
        /// </summary>
        public bool IsUseLYApi { get; set; }

        /// <summary>
        /// 是否使用EUA微信授权
        /// </summary>
        public bool IsUseLYWxOauth2 { get; set; }

        /// <summary>
        /// EUA登录模式:本地Local,单点SingleSign,共享StateServer
        /// </summary>
        public string LYLoginModel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LYStateServer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LYApiUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LYAppId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LYAppSecret { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LYAppToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LogFilePath { get; set; }
    }
}
