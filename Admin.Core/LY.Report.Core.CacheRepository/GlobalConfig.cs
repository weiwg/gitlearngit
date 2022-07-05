using LY.Report.Core.CacheRepository.Enum;
using LY.Report.Core.Common.Configs;
using LY.Report.Core.Util.Common;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.CacheRepository
{
    /// <summary>
    /// 全局配置
    /// </summary>
    public class GlobalConfig
    {
        #if DEBUG
        private static AppConfig AppConfig = ConfigHelper.Get<AppConfig>("appconfig", "Development") ?? new AppConfig();
        private static LYAuthConfig LYAuthConfig => ConfigHelper.Get<LYAuthConfig>("lyauthconfig", "Development") ?? new LYAuthConfig();
        #else
        private static AppConfig AppConfig = ConfigHelper.Get<AppConfig>("appconfig") ?? new AppConfig();
        private static EupAuthConfig EupAuthConfig => ConfigHelper.Get<EupAuthConfig>("eupauthconfig") ?? new EupAuthConfig();
        #endif

        private static bool _isInit;

        #region 统一授权
        private static bool _isUseLYApi;
        /// <summary>
        /// 是否使用EUA API
        /// </summary>
        public static bool IsUseEupApi { get { CheckInit(); return _isUseLYApi; } }

        private static bool _isUseLYWxOauth2;
        /// <summary>
        /// 是否使用EUA微信授权
        /// </summary>
        public static bool IsUseEupWxOauth2 { get { CheckInit(); return _isUseLYWxOauth2; } }

        private static LYLoginModel _LYLoginModel;
        /// <summary>
        /// EUA登录模式:本地Local,单点SingleSign,共享StateServer
        /// </summary>
        public static LYLoginModel LYLoginModel { get { CheckInit(); return _LYLoginModel; } }

        private static string _LYStateServer;
        /// <summary>
        /// 使用EUA共享Session登录,LYLoginModel为StateServer时有效
        /// </summary>
        public static string LYStateServer { get { CheckInit(); return _LYStateServer; } }

        private static string _LYApiUrl;
        /// <summary>
        /// LYApiUrl
        /// </summary>
        public static string LYApiUrl { get { CheckInit(); return _LYApiUrl; } }

        private static string _LYAppId;
        /// <summary>
        /// LYAppId
        /// </summary>
        public static string LYAppId { get { CheckInit(); return _LYAppId; } }

        private static string _LYAppSecret;
        /// <summary>
        /// LYAppSecret
        /// </summary>
        public static string LYAppSecret { get { CheckInit(); return _LYAppSecret; } }

        private static string _lyAppToken;
        /// <summary>
        /// LYAppToken
        /// </summary>
        public static string LYAppToken { get { CheckInit(); return _lyAppToken; } }
        #endregion

        private static int _cacheExpiresTime;
        /// <summary>
        /// 缓存过期时间(秒)
        /// </summary>
        public static int CacheExpiresTime { get { CheckInit(); return _cacheExpiresTime; } }

        private static string _baiduMapAppKey;
        /// <summary>
        /// 百度地图Key
        /// </summary>
        public static string BaiduMapAppKey { get { CheckInit(); return _baiduMapAppKey; } }

        private static string _qqMapAppKey;
        /// <summary>
        /// 腾讯地图Key
        /// </summary>
        public static string QqMapAppKey { get { CheckInit(); return _qqMapAppKey; } }

        private static string _amapMapAppKey;
        /// <summary>
        /// 高德地图Web Api Key
        /// </summary>
        public static string AmapMapAppKey { get { CheckInit(); return _amapMapAppKey; } }

        private static string _mallApiUrl;
        /// <summary>
        /// 商城api url
        /// </summary>
        public static string MallApiUrl { get { CheckInit(); return _mallApiUrl; } }

        private static string _payApiUrl;
        /// <summary>
        /// 支付网关api url
        /// </summary>
        public static string PayApiUrl { get { CheckInit(); return _payApiUrl; } }

        /// <summary>
        /// 检查是否已初始化
        /// </summary>
        private static void CheckInit() { if (!_isInit) { Init(); } }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            _isInit = true;

            _isUseLYApi = LYAuthConfig.IsUseLYApi;
            _isUseLYWxOauth2 = LYAuthConfig.IsUseLYWxOauth2;
            _LYLoginModel = EnumHelper.GetEnumModel<LYLoginModel>(LYAuthConfig.LYLoginModel);
            _LYStateServer = LYAuthConfig.LYStateServer;
            _LYApiUrl = LYAuthConfig.LYApiUrl;
            _LYAppId = LYAuthConfig.LYAppId;
            _LYAppSecret = LYAuthConfig.LYAppSecret;
            _lyAppToken = LYAuthConfig.LYAppToken;

            _cacheExpiresTime = CommonHelper.GetInt(AppConfig.CacheExpiresTime);
            _baiduMapAppKey = AppConfig.Apis.BaiduMapAppKey;
            _qqMapAppKey = AppConfig.Apis.QqMapAppKey;
            _amapMapAppKey = AppConfig.Apis.AmapMapAppKey;
            _mallApiUrl = AppConfig.Apis.MallApiUrl;
            _payApiUrl = AppConfig.Apis.PayApiUrl;
        }
    }
}
