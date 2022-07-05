/* ******************************************************
 * 作者：weig
 * 功能：请求API接口
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20191024 weigang  创建   
 ***************************************************** */

using System.Collections;
using System.Web;
using LY.UnifiedAuth.Util.Api.Core.Configs;
using LY.UnifiedAuth.Util.Api.Core.Entity;
using LY.UnifiedAuth.Util.Api.Core.Util;

namespace LY.UnifiedAuth.Util.Api.Core
{
    /// <summary>
    /// 请求API接口
    /// </summary>
    public class UaApi
    {
        #if DEBUG
        private static LYAuthConfig LYAuthConfig => ApiConfigHelper.Get<LYAuthConfig>("lyauthconfig", "Development") ?? new LYAuthConfig();
        #else
        private static EupAuthConfig EupAuthConfig => ApiConfigHelper.Get<EupAuthConfig>("eupauthconfig") ?? new EupAuthConfig();
        #endif

        #region
        #endregion

        #region 配置参数
        /// <summary>
        /// ApiHostUrl
        /// </summary>
        public static string ApiHostUrl = LYAuthConfig.LYApiUrl;
        /// <summary>
        /// ApiAppId
        /// </summary>
        public static string ApiAppId = LYAuthConfig.LYAppId;
        /// <summary>
        /// ApiAppSecret
        /// </summary>
        public static string ApiAppSecret = LYAuthConfig.LYAppSecret;
        #endregion

        #region 接口连接
        /// <summary>
        /// 保持连接状态
        /// </summary>
        /// <returns></returns>
        public static string GetUrlAlive()
        {
            return string.Format("{0}/api/user/alive", ApiHostUrl);
        }

        /// <summary>
        /// 检查登录状态js
        /// </summary>
        /// <param name="logoutUrl">主动登出路径</param>
        /// <returns></returns>
        public static string GetUrlCheckLogin(string logoutUrl = "")
        {
            return string.Format("{0}/api/user/checklogin{1}", ApiHostUrl, string.IsNullOrEmpty(logoutUrl) ? "" : "?logouturl=" + HttpUtility.UrlEncode(logoutUrl));
        }

        /// <summary>
        /// 获取登录路径
        /// </summary>
        /// <param name="returnUrl">回调路径</param>
        /// <returns></returns>
        public static string GetUrlLogin(string returnUrl)
        {
            return string.Format("{0}/account/login?returnurl={1}", ApiHostUrl, HttpUtility.UrlEncode(returnUrl));
        }

        /// <summary>
        /// 获取登出路径
        /// </summary>
        /// <param name="returnUrl">回调路径</param>
        /// <param name="sessionId">用户标记id</param>
        /// <returns></returns>
        public static string GetUrlLogout(string returnUrl = "", string sessionId = "")
        {
            string url = string.Format("{0}/account/logout{1}", ApiHostUrl, string.IsNullOrEmpty(returnUrl) ? "" : "?returnurl=" + HttpUtility.UrlEncode(returnUrl));

            if (!string.IsNullOrEmpty(sessionId))
            {
                url = string.Format("{0}{1}{2}", url, string.IsNullOrEmpty(returnUrl) ? "?" : "&", "sessionid=" + sessionId);
            }
            return url;
        }

        /// <summary>
        /// 获取微信Oauth2路径
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="returnUrl">回调路径</param>
        /// <param name="type">
        /// 请求类型
        /// applyid:返回企业编号</param>
        /// <param name="isDebug">是否调试模式</param>
        /// <returns></returns>
        public static string GetUrlWxOauth2(string apiAccessToken, string returnUrl, string type = "", bool isDebug = false)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/weixin/oauth2?token={1}&sign={2}&ts={3}&redirecturl={4}", ApiHostUrl, apiAccessToken, signature, timestamp, HttpUtility.UrlEncode(returnUrl));
            if (!string.IsNullOrEmpty(type))
            {
                apiUrl += string.Format("&type={0}", type.ToLower());
            }
            if (isDebug)
            {
                apiUrl += "&db=true";
            }
            return apiUrl;
        }

        #endregion

        #region API AccessToken
        /// <summary>
        /// 获取API AccessToken
        /// </summary>
        /// <param name="isDebug">调试状态不刷新AccessToken,以便本地多人调试</param>
        /// <returns></returns>
        public static ApiResult GetApiAccessToken(bool isDebug = false)
        {
            return GetApiAccessToken(ApiAppId, ApiAppSecret, isDebug);
        }

        /// <summary>
        /// 获取API AccessToken
        /// </summary>
        /// <param name="apiAppId">AppId</param>
        /// <param name="apiAppSecret">AppSecret</param>
        /// <param name="isDebug">调试状态不刷新AccessToken,以便本地多人调试</param>
        /// <returns></returns>
        public static ApiResult GetApiAccessToken(string apiAppId, string apiAppSecret, bool isDebug = false)
        {
            string apiUrl = string.Format("{0}/api/auth/apiaccesstoken", ApiHostUrl);
            apiUrl = UnifiedAuthApiHelper.CreatApiSignatureUrl(apiUrl, ApiAppId, ApiAppSecret);
            if (isDebug)
            {
                apiUrl = apiUrl + "&refresh=false";

            }
            return RequestApi.GetApi(apiUrl);
        }

        /// <summary>
        /// 校验系统认证令牌
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="authToken">API系统认证令牌</param>
        /// <returns></returns>
        public static ApiResult CheckAuthToken(string apiAccessToken, string authToken)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/auth/checkauthtoken?token={1}&sign={2}&ts={3}&authtoken={4}", ApiHostUrl, apiAccessToken, signature, timestamp, authToken);
            return RequestApi.GetApi(apiUrl, GetHeaders(apiAccessToken));
        }

        /// <summary>
        /// 校验系统认证令牌(Jwt)
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="authJwtToken">API系统认证Jwt令牌</param>
        /// <returns></returns>
        public static ApiResult CheckAuthJwtToken(string apiAccessToken, string authJwtToken)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/auth/checkauthjwttoken?token={1}&sign={2}&ts={3}&authjwttoken={4}", ApiHostUrl, apiAccessToken, signature, timestamp, authJwtToken);
            return RequestApi.GetApi(apiUrl, GetHeaders(apiAccessToken));
        }
        #endregion

        #region 用户

        #region 获取用户信息
        /// <summary>
        /// 校验LoginToken并获取用户信息
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="loginToken">API登录令牌</param>
        /// <returns></returns>
        public static ApiResult CheckLogin(string apiAccessToken, string loginToken)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/auth/checklogin?token={1}&sign={2}&ts={3}&logintoken={4}", ApiHostUrl, apiAccessToken, signature, timestamp, loginToken);
            return RequestApi.GetApi(apiUrl, GetHeaders(apiAccessToken));
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="unifiedUserId">用户id</param>
        /// <returns></returns>
        public static ApiResult GetUserInfo(string apiAccessToken, string unifiedUserId)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/user/userinfo?token={1}&sign={2}&ts={3}&uuid={4}", ApiHostUrl, apiAccessToken, signature, timestamp, unifiedUserId);
            return RequestApi.GetApi(apiUrl, GetHeaders(apiAccessToken));
        }
        #endregion

        #region 获取用户微信信息
        /// <summary>
        /// 获取用户微信信息
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="openId">openId</param>
        /// <returns></returns>
        public static ApiResult GetUserWxInfo(string apiAccessToken, string openId)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/weixin/userinfo?token={1}&sign={2}&ts={3}&openid={4}", ApiHostUrl, apiAccessToken, signature, timestamp, openId);
            return RequestApi.GetApi(apiUrl, GetHeaders(apiAccessToken));
        }
        #endregion

        #region 用户注册
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="appToken">API AppToken</param>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"UserName":"{UserName}","Password":"{Password}","NickName":"{NickName}","Phone":"{Phone}","Email":"{Email}"}
        /// Password:明文密码
        /// Phone,Email:必填一个,或两个都填
        /// </param>
        /// <returns></returns>
        public static ApiResult Register(string apiAccessToken, string appToken, string postData)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/user/register?token={1}&sign={2}&ts={3}&encrypt=true", ApiHostUrl, apiAccessToken, signature, timestamp);
            return RequestApi.PostApi(apiUrl, postData, appToken);
        }
        #endregion

        #region 修改用户信息
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="appToken">API AppToken</param>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"UnifiedUserId":"{UnifiedUserId}","NickName":"{NickName}","Portrait":"{Portrait}"}
        /// </param>
        /// <returns></returns>
        public static ApiResult UpdateUserInfo(string apiAccessToken, string appToken, string postData)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/user/updateuserinfo?token={1}&sign={2}&ts={3}&encrypt=true", ApiHostUrl, apiAccessToken, signature, timestamp);
            return RequestApi.PostApi(apiUrl, postData, appToken);
        }
        #endregion

        #region 修改用户账号(手机号或邮箱)
        /// <summary>
        /// 修改用户账号(手机号或邮箱)
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="appToken">API AppToken</param>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"UnifiedUserId":"{UnifiedUserId}","Account":"{Account}"}
        /// </param>
        /// <returns></returns>
        public static ApiResult UpdateUserAccount(string apiAccessToken, string appToken, string postData)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/user/updateaccount?token={1}&sign={2}&ts={3}&encrypt=true", ApiHostUrl, apiAccessToken, signature, timestamp);
            return RequestApi.PostApi(apiUrl, postData, appToken);
        }
        #endregion

        #region 修改用户登录密码
        /// <summary>
        /// 修改用户登录密码
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="appToken">API AppToken</param>
        /// <param name="postData">
        /// 请求数据json格式
        /// 旧密码不为空,校验旧密码
        /// {"UnifiedUserId":"{UnifiedUserId}","Password":"{Password}","OldPassword":"{OldPassword}"}
        /// </param>
        /// <returns></returns>
        public static ApiResult UpdateUserPassword(string apiAccessToken, string appToken, string postData)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/user/updatepassword?token={1}&sign={2}&ts={3}&encrypt=true", ApiHostUrl, apiAccessToken, signature, timestamp);
            return RequestApi.PostApi(apiUrl, postData, appToken);
        }
        #endregion

        #region 校验用户登录密码
        /// <summary>
        /// 校验用户登录密码
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="appToken">API AppToken</param>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"UnifiedUserId":"{UnifiedUserId}","Password":"{Password}"}
        /// </param>
        /// <returns></returns>
        public static ApiResult CheckPassword(string apiAccessToken, string appToken, string postData)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/user/checkpassword?token={1}&sign={2}&ts={3}&encrypt=true", ApiHostUrl, apiAccessToken, signature, timestamp);
            return RequestApi.PostApi(apiUrl, postData, appToken);
        }
        #endregion

        #region 用户登录校验
        /// <summary>
        /// 用户登录校验,并返回用户信息
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="appToken">API AppToken</param>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"UserName":"{UserName}","Password":"{Password}"}
        /// </param>
        /// <returns></returns>
        public static ApiResult UserLogin(string apiAccessToken, string appToken, string postData)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/user/checkuserlogin?token={1}&sign={2}&ts={3}&encrypt=true", ApiHostUrl, apiAccessToken, signature, timestamp);
            return RequestApi.PostApi(apiUrl, postData, appToken);
        }
        #endregion

        #region 绑定微信
        /// <summary>
        /// 绑定微信
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="appToken">API AppToken</param>
        /// <param name="postData">
        /// 请求数据json格式
        /// OpenId 为空>解绑
        /// {"UnifiedUserId":"{UnifiedUserId}","OpenId":"{OpenId}"}
        /// </param>
        /// <returns></returns>
        public static ApiResult BindWeChat(string apiAccessToken, string appToken, string postData)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/user/bindwechat?token={1}&sign={2}&ts={3}&encrypt=true", ApiHostUrl, apiAccessToken, signature, timestamp);
            return RequestApi.PostApi(apiUrl, postData, appToken);
        }
        #endregion

        #endregion

        #region XERP

        #region 用户XERP绑定
        /// <summary>
        /// 绑定Xerp用户
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="appToken">API AppToken</param>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"ApplyId":"{ApplyId}","Account":"{Account}","Password":"{Password}","OpenId":"{OpenId}","ParentUserId":"{ParentUserId}",
        /// "ParentApplyId":"{ParentApplyId}","ParentAccount":"{ParentAccount},"ParentOpenId":"{ParentOpenId}"}
        /// ParentApplyId,ParentAccount 与 ParentUserId 二选一
        /// </param>
        /// <returns></returns>
        public static ApiResult BindXerpUser(string apiAccessToken, string appToken, string postData)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/xerp/binduser?token={1}&sign={2}&ts={3}&encrypt=true", ApiHostUrl, apiAccessToken, signature, timestamp);
            return RequestApi.PostApi(apiUrl, postData, appToken);
        }
        #endregion

        #region 解绑Xerp用户
        /// <summary>
        /// 解绑Xerp用户
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="appToken">API AppToken</param>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"UserId":"{UserId}",{"LinkId":"{LinkId}"}
        /// </param>
        /// <returns></returns>
        public static ApiResult UnBindXerpUser(string apiAccessToken, string appToken, string postData)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/xerp/unbinduser?token={1}&sign={2}&ts={3}&encrypt=true", ApiHostUrl, apiAccessToken, signature, timestamp);
            return RequestApi.PostApi(apiUrl, postData, appToken);
        }
        #endregion

        #region 获取用户XERP绑定信息
        /// <summary>
        /// 获取用户XERP绑定信息
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="applyId">企业编号(与unifiedUserId二选一)</param>
        /// <param name="account">登录名(与unifiedUserId二选一)</param>
        /// <param name="unifiedUserId">用户id</param>
        /// <returns></returns>
        public static ApiResult GetBindXerpUser(string apiAccessToken, string applyId, string account, string unifiedUserId = "")
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/xerp/getbinduser?token={1}&sign={2}&ts={3}&uuid={4}&applyid={5}&account={6}", ApiHostUrl, apiAccessToken, signature, timestamp, unifiedUserId, applyId, account);
            return RequestApi.GetApi(apiUrl, GetHeaders(apiAccessToken));
        }
        #endregion

        #region 更新Xerp用户配置
        /// <summary>
        /// 更新Xerp用户配置
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"OpenId":"{OpenId}","ApplyId":"{ApplyId}","UserType":"{UserType}"}
        /// UserType 选填
        /// </param>
        /// <returns></returns>
        public static ApiResult UpdateUserXerpConfig(string apiAccessToken, string postData)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/xerp/userxerpconfig?token={1}&sign={2}&ts={3}", ApiHostUrl, apiAccessToken, signature, timestamp);
            return RequestApi.PostApi(apiUrl, postData);
        }
        #endregion

        #endregion

        #region 微信

        #region 获取微信AccessToken
        /// <summary>
        /// 获取微信AccessToken
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <returns></returns>
        public static ApiResult GetWxAccessToken(string apiAccessToken)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/weixin/accesstoken?token={1}&sign={2}&ts={3}", ApiHostUrl, apiAccessToken, signature, timestamp);
            return RequestApi.GetApi(apiUrl, GetHeaders(apiAccessToken));
        }
        #endregion

        #region 获取微信Ticket
        /// <summary>
        /// 获取微信Ticket
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <returns></returns>
        public static ApiResult GetWxTicket(string apiAccessToken)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/weixin/ticket?token={1}&sign={2}&ts={3}", ApiHostUrl, apiAccessToken, signature, timestamp);
            return RequestApi.GetApi(apiUrl, GetHeaders(apiAccessToken));
        }
        #endregion

        #region 获取JS-SDK权限验证的签名
        /// <summary>
        /// 获取JS-SDK权限验证的签名Signature
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="url">当前网页的url，不包含#及其后面部分</param>
        /// <returns></returns>
        public static ApiResult GetWxJsSdkSignature(string apiAccessToken, string url)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/weixin/jssdksignature?token={1}&sign={2}&ts={3}&url={4}", ApiHostUrl, apiAccessToken, signature, timestamp, HttpUtility.UrlEncode(url));
            return RequestApi.GetApi(apiUrl, GetHeaders(apiAccessToken));
        }
        #endregion

        #region 获取微信定位Url
        /// <summary>
        /// 获取微信定位Url
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="coordinateType">坐标类型(默认wgs84,可选gcj02)</param>
        /// <returns></returns>
        public static string GetUrlWxLocation(string apiAccessToken, string coordinateType = "wgs84")
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            return string.Format("{0}/api/weixin/location?token={1}&sign={2}&ts={3}&type={4}", ApiHostUrl, apiAccessToken, signature, timestamp, coordinateType);
        }
        #endregion

        #endregion

        #region 消息发送

        #region 发送验证码(手机/邮箱)
        /// <summary>
        /// 发送验证码(手机/邮箱)
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"Account":"{Account}","Code":"{Code}","ExpiresIn":"{ExpiresIn}"}
        /// 发送账号Account:手机号或邮箱
        /// 验证码Code
        /// 验证码有效时间(秒)ExpiresIn:仅作前台展示
        /// </param>
        /// <returns></returns>
        public static ApiResult SendSecurityCode(string apiAccessToken, string postData)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/msg/sendsecuritycode?token={1}&sign={2}&ts={3}", ApiHostUrl, apiAccessToken, signature, timestamp);
            return RequestApi.PostApi(apiUrl, postData);
        }
        #endregion

        #region 发送自定义短信
        /// <summary>
        /// 发送自定义短信
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"Phones":"{Phones}","SignName":"{SignName}","TemplateCode":"{TemplateCode}","TemplateParam":"{TemplateParam}"}
        /// 手机号Phones:支持对多个手机号码发送短信，手机号码之间以英文逗号（,）分隔
        /// 内容Content
        /// </param>
        /// <returns></returns>
        public static ApiResult SendSms(string apiAccessToken, string postData)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/msg/sendsms?token={1}&sign={2}&ts={3}", ApiHostUrl, apiAccessToken, signature, timestamp);
            return RequestApi.PostApi(apiUrl, postData);
        }
        #endregion

        #region 发送自定义邮件
        /// <summary>
        /// 发送自定义邮件
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"Subject":"{Subject}","Content":"{Content}","Reciver":"{Reciver}"}
        /// 选填:MsgUrl,SendMode,MsgColor
        /// 邮件标题Subjec
        /// 邮件内容Content
        /// 收件地址Reciver
        /// </param>
        /// <returns></returns>
        public static ApiResult SendEmail(string apiAccessToken, string postData)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/msg/sendemail?token={1}&sign={2}&ts={3}", ApiHostUrl, apiAccessToken, signature, timestamp);
            return RequestApi.PostApi(apiUrl, postData);
        }
        #endregion

        #region 发送微信模板消息
        /// <summary>
        /// 发送微信模板消息
        /// </summary>
        /// <param name="apiAccessToken">API访问令牌</param>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"ToOpenId":"{ToOpenId}","TemplateId":"{TemplateId}","Msg":"{Msg}","MsgUrl":"{SendMode}","SendMode":"{SendMode}","MsgColor":"{MsgColor}"}
        /// 选填:MsgUrl,SendMode,MsgColor
        /// 发送模式SendMode:now实时,day白天
        /// </param>
        /// <returns></returns>
        public static ApiResult SendWeChat(string apiAccessToken, string postData)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/msg/sendwechat?token={1}&sign={2}&ts={3}", ApiHostUrl, apiAccessToken, signature, timestamp);
            return RequestApi.PostApi(apiUrl, postData);
        }
        #endregion

        #endregion

        #region Jwt
        /// <summary>
        /// 校验Jwt Token
        /// </summary>
        /// <param name="apiAccessToken"></param>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public static ApiResult ValidateJwtToken(string apiAccessToken, string jwtToken)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/auth/validatejwttoken?token={1}&sign={2}&ts={3}&jwttoken={4}", ApiHostUrl, apiAccessToken, signature, timestamp, jwtToken);
            return RequestApi.GetApi(apiUrl, GetHeaders(apiAccessToken));
        }

        /// <summary>
        /// 刷新Jwt Token
        /// </summary>
        /// <param name="apiAccessToken"></param>
        /// <param name="oldJwtToken"></param>
        /// <returns></returns>
        public static ApiResult RefreshJwtToken(string apiAccessToken, string oldJwtToken)
        {
            long timestamp = UnifiedAuthApiHelper.GetTimestamp();
            string signature = "";//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp);
            string apiUrl = string.Format("{0}/api/auth/refreshjwttoken?token={1}&sign={2}&ts={3}&jwttoken={4}", ApiHostUrl, apiAccessToken, signature, timestamp, oldJwtToken);
            return RequestApi.GetApi(apiUrl, GetHeaders(apiAccessToken));
        }
        #endregion

        #region 内部方法

        private static Hashtable GetHeaders(string apiAccessToken)
        {
            //return null;
            return new Hashtable
            {
                //    //["Ua-AppId"] = appId,
                //    ["Ua-Timestamp"] = UnifiedAuthApiHelper.GetTimestamp(),
                ["Ua-Token"] = apiAccessToken,
                //    ["Ua-Signature"] = ""//UnifiedAuthApiHelper.GetApiDataSignature(apiAccessToken, out timestamp)
            };
        }
        #endregion
    }
}
