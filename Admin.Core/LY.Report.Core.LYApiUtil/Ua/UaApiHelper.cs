using LY.Report.Core.CacheRepository;
using LY.Report.Core.Util.Common;
using LY.UnifiedAuth.Util.Api.Core;
using LY.UnifiedAuth.Util.Api.Core.Entity;

namespace LY.Report.Core.LYApiUtil.Ua
{
    /// <summary>
    /// 接口帮助类
    /// 修改时间:2019-12-10 17:23
    /// </summary>
    public class UaApiHelper
    {
        private readonly LogHelper _logger = new LogHelper("UaApiHelper");

        #region 接口连接
        /// <summary>
        /// 保持连接状态
        /// </summary>
        /// <returns></returns>
        public static string GetUrlAlive()
        {
            return UaApi.GetUrlAlive();
        }

        /// <summary>
        /// 检查登录状态js
        /// </summary>
        /// <param name="logoutUrl">主动登出路径</param>
        /// <returns></returns>
        public static string GetUrlCheckLogin(string logoutUrl = "")
        {
            return UaApi.GetUrlCheckLogin(logoutUrl);
        }

        /// <summary>
        /// 获取登出路径
        /// </summary>
        /// <param name="returnUrl">跳转路径</param>
        /// <returns></returns>
        public static string GetUrlLogin(string returnUrl = "")
        {
            return UaApi.GetUrlLogin(returnUrl);
        }

        /// <summary>
        /// 获取登出路径
        /// </summary>
        /// <param name="returnUrl">跳转路径</param>
        /// <param name="sessionId">用户标记id</param>
        /// <returns></returns>
        public static string GetUrlLogout(string returnUrl = "", string sessionId = "")
        {
            return UaApi.GetUrlLogout(returnUrl, sessionId);
        }

        /// <summary>
        /// 获取微信Oauth2路径
        /// </summary>
        /// <param name="redirectUrl">回调路径</param>
        /// <param name="type">
        /// 请求类型
        /// applyid:返回企业编号</param>
        /// <param name="isDebug">是否调试模式</param>
        /// <returns></returns>
        public static string GetUrlWxOauth2(string redirectUrl, string type = "", bool isDebug = false)
        {
            return UaApi.GetUrlWxOauth2(ApiTokenHelper.GetApiAccessToken(), redirectUrl, type, isDebug);
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
            var apiResult = UaApi.GetApiAccessToken(isDebug);
            return apiResult;
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
            var apiResult = UaApi.GetApiAccessToken(apiAppId, apiAppSecret, isDebug);
            return apiResult;
        }

        /// <summary> 
        /// 校验系统认证令牌
        /// </summary>
        /// <param name="authToken">API系统认证令牌</param>
        /// <returns></returns>
        public static ApiResult CheckAuthToken(string authToken)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.CheckAuthToken(apiAccessToken, authToken);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.CheckAuthToken(apiAccessToken, authToken);
            }
            return apiResult;
        }
        #endregion

        #region 用户
        
        #region 获取用户信息
        /// <summary>
        /// 校验LoginToken并获取用户信息
        /// </summary>
        /// <param name="loginToken">API登录令牌</param>
        /// <returns></returns>
        public static ApiResult CheckLogin(string loginToken)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.CheckLogin(apiAccessToken, loginToken);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.CheckLogin(apiAccessToken, loginToken);
            }
            return apiResult;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="unifiedUserId">用户id</param>
        /// <returns></returns>
        public static ApiResult GetUserInfo(string unifiedUserId)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.GetUserInfo(apiAccessToken, unifiedUserId);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.GetUserInfo(apiAccessToken, unifiedUserId);
            }
            return apiResult;
        }
        #endregion

        #region 获取用户微信信息
        /// <summary>
        /// 获取用户微信信息
        /// </summary>
        /// <param name="openId">openId</param>
        /// <returns></returns>
        public static ApiResult GetUserWxInfo(string openId)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.GetUserWxInfo(apiAccessToken, openId);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.GetUserWxInfo(apiAccessToken, openId);
            }
            return apiResult;
        }
        #endregion

        #region 用户注册
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="appToken">API AppToken</param>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"UserName":"{UserName}","Password":"{Password}","NickName":"{NickName}","Phone":"{Phone}","Email":"{Email}"}
        /// Password:明文密码
        /// Phone,Email:必填一个,或两个都填
        /// </param>
        /// <returns></returns>
        public static ApiResult Register(string appToken, string postData)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.Register(apiAccessToken, GlobalConfig.LYAppToken, postData);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.Register(apiAccessToken, GlobalConfig.LYAppToken, postData);
            }
            return apiResult;
        }
        #endregion

        #region 修改用户信息
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"UnifiedUserId":"{UnifiedUserId}","NickName":"{NickName}","Portrait":"{Portrait}"}
        /// </param>
        /// <returns></returns>
        public static ApiResult UpdateUserInfo(string postData)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.UpdateUserInfo(apiAccessToken, GlobalConfig.LYAppToken, postData);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.UpdateUserInfo(apiAccessToken, GlobalConfig.LYAppToken, postData);
            }
            return apiResult;
        }
        #endregion

        #region 修改用户账号(手机号或邮箱)
        /// <summary>
        /// 修改用户账号(手机号或邮箱)
        /// </summary>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"UnifiedUserId":"{UnifiedUserId}","Account":"{Account}"}
        /// </param>
        /// <returns></returns>
        public static ApiResult UpdateUserAccount(string postData)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.UpdateUserAccount(apiAccessToken, GlobalConfig.LYAppToken, postData);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.UpdateUserAccount(apiAccessToken, GlobalConfig.LYAppToken, postData);
            }
            return apiResult;
        }
        #endregion

        #region 修改用户登录密码
        /// <summary>
        /// 修改用户登录密码
        /// </summary>
        /// <param name="postData">
        /// 请求数据json格式
        /// 旧密码不为空,校验旧密码
        /// {"UnifiedUserId":"{UnifiedUserId}","Password":"{Password}","OldPassword":"{OldPassword}"}
        /// </param>
        /// <returns></returns>
        public static ApiResult UpdateUserPassword(string postData)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.UpdateUserPassword(apiAccessToken, GlobalConfig.LYAppToken, postData);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.UpdateUserPassword(apiAccessToken, GlobalConfig.LYAppToken, postData);
            }
            return apiResult;
        }
        #endregion

        #region 校验用户登录密码
        /// <summary>
        /// 校验用户登录密码
        /// </summary>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"UnifiedUserId":"{UnifiedUserId}","Password":"{Password}"}
        /// </param>
        /// <returns></returns>
        public static ApiResult CheckPassword(string postData)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.CheckPassword(apiAccessToken, GlobalConfig.LYAppToken, postData);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.CheckPassword(apiAccessToken, GlobalConfig.LYAppToken, postData);
            }
            return apiResult;
        }
        #endregion

        #region 用户登录校验
        /// <summary>
        /// 用户登录校验,并返回用户信息
        /// </summary>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"UserName":"{UserName}","Password":"{Password}"}
        /// </param>
        /// <returns></returns>
        public static ApiResult UserLogin(string postData)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.UserLogin(apiAccessToken, GlobalConfig.LYAppToken, postData);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.UserLogin(apiAccessToken, GlobalConfig.LYAppToken, postData);
            }
            return apiResult;
        }
        #endregion

        #region 绑定微信
        /// <summary>
        /// 绑定微信
        /// </summary>
        /// <param name="postData">
        /// 请求数据json格式
        /// OpenId 为空>解绑
        /// {"UnifiedUserId":"{UnifiedUserId}","OpenId":"{OpenId}"}
        /// </param>
        /// <returns></returns>
        public static ApiResult BindWeChat(string postData)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.BindWeChat(apiAccessToken, GlobalConfig.LYAppToken, postData);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.BindWeChat(apiAccessToken, GlobalConfig.LYAppToken, postData);
            }
            return apiResult;
        }
        #endregion

        #endregion

        #region XERP

        #region 绑定Xerp用户
        /// <summary>
        /// 绑定Xerp用户
        /// </summary>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"ApplyId":"{ApplyId}","Account":"{Account}","Password":"{Password}","OpenId":"{OpenId}","ParentUserId":"{ParentUserId}",
        /// "ParentApplyId":"{ParentApplyId}","ParentAccount":"{ParentAccount},"ParentOpenId":"{ParentOpenId}"}
        /// ParentApplyId,ParentAccount 与 ParentUserId 二选一
        /// </param>
        /// <returns></returns>
        public static ApiResult BindXerpUser(string postData)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.BindXerpUser(apiAccessToken, GlobalConfig.LYAppToken, postData);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.BindXerpUser(apiAccessToken, GlobalConfig.LYAppToken, postData);
            }
            return apiResult;
        }
        #endregion

        #region 解绑Xerp用户
        /// <summary>
        /// 解绑Xerp用户
        /// </summary>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"UserId":"{UserId}",{"LinkId":"{LinkId}"}
        /// </param>
        /// <returns></returns>
        public static ApiResult UnBindXerpUser(string postData)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.UnBindXerpUser(apiAccessToken, GlobalConfig.LYAppToken, postData);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.UnBindXerpUser(apiAccessToken, GlobalConfig.LYAppToken, postData);
            }
            return apiResult;
        }
        #endregion

        #region 获取用户XERP绑定信息
        /// <summary>
        /// 获取用户XERP绑定信息
        /// </summary>
        /// <param name="applyId">企业编号(与unifiedUserId二选一)</param>
        /// <param name="account">登录名(与unifiedUserId二选一)</param>
        /// <param name="unifiedUserId">用户id</param>
        /// <returns></returns>
        public static ApiResult GetBindXerpUser(string applyId, string account, string unifiedUserId = "")
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.GetBindXerpUser(apiAccessToken, applyId, account, unifiedUserId);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.GetBindXerpUser(apiAccessToken, applyId, account, unifiedUserId);
            }
            return apiResult;
        }
        #endregion

        #region 更新Xerp用户配置
        /// <summary>
        /// 更新Xerp用户配置
        /// </summary>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"OpenId":"{OpenId}","ApplyId":"{ApplyId}","UserType":"{UserType}"}
        /// UserType 选填
        /// </param>
        /// <returns></returns>
        public static ApiResult UpdateUserXerpConfig(string postData)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.UpdateUserXerpConfig(apiAccessToken, postData);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.UpdateUserXerpConfig(apiAccessToken, postData);
            }
            return apiResult;
        }
        #endregion

        #endregion

        #region 微信

        #region 获取微信AccessToken
        /// <summary>
        /// 获取微信AccessToken
        /// </summary>
        /// <returns></returns>
        public static ApiResult GetWxAccessToken()
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.GetWxAccessToken(apiAccessToken);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.GetWxAccessToken(apiAccessToken);
            }
            return apiResult;
        }
        #endregion

        #region 获取微信Ticket
        /// <summary>
        /// 获取微信Ticket
        /// </summary>
        /// <returns></returns>
        public static ApiResult GetWxTicket()
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.GetWxTicket(apiAccessToken);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.GetWxTicket(apiAccessToken);
            }
            return apiResult;
        }
        #endregion

        #region 获取JS-SDK权限验证的签名
        /// <summary>
        /// 获取JS-SDK权限验证的签名Signature
        /// </summary>
        /// <param name="url">当前网页的url，不包含#及其后面部分</param>
        /// <returns></returns>
        public static ApiResult GetWxJsSdkSignature(string url)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.GetWxJsSdkSignature(apiAccessToken, url);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.GetWxJsSdkSignature(apiAccessToken, url);
            }
            return apiResult;
        }
        #endregion

        #region 获取微信定位Url
        /// <summary>
        /// 获取微信定位Url
        /// </summary>
        /// <param name="coordinateType">坐标类型(默认wgs84,可选gcj02)</param>
        /// <returns></returns>
        public static string GetUrlWxLocation(string coordinateType = "wgs84")
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            return UaApi.GetUrlWxLocation(apiAccessToken, coordinateType);
        }
        #endregion

        #endregion

        #region 消息发送

        #region 发送验证码(手机/邮箱)
        /// <summary>
        /// 发送验证码(手机/邮箱)
        /// </summary>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"Account":"{Account}","Code":"{Code}","ExpiresIn":"{ExpiresIn}"}
        /// 发送账号Account:手机号或邮箱
        /// 验证码Code
        /// 验证码有效时间(秒)ExpiresIn:仅作前台展示
        /// </param>
        /// <returns></returns>
        public static ApiResult SendSecurityCode(string postData)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.SendSecurityCode(apiAccessToken, postData);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.SendSecurityCode(apiAccessToken, postData);
            }
            return apiResult;
        }
        #endregion

        #region 发送自定义短信
        /// <summary>
        /// 发送自定义短信
        /// </summary>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"Phones":"{Phones}","SignName":"{SignName}","TemplateCode":"{TemplateCode}","TemplateParam":"{TemplateParam}"}
        /// 手机号Phones:支持对多个手机号码发送短信，手机号码之间以英文逗号（,）分隔
        /// 内容Content
        /// </param>
        /// <returns></returns>
        public static ApiResult SendSms(string postData)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.SendSms(apiAccessToken, postData);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.SendSms(apiAccessToken, postData);
            }
            return apiResult;
        }
        #endregion

        #region 发送自定义邮件
        /// <summary>
        /// 发送自定义邮件
        /// </summary>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"Subject":"{Subject}","Content":"{Content}","Reciver":"{Reciver}"}
        /// 选填:MsgUrl,SendMode,MsgColor
        /// 邮件标题Subjec
        /// 邮件内容Content
        /// 收件地址Reciver
        /// </param>
        /// <returns></returns>
        public static ApiResult SendEmail(string postData)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.SendEmail(apiAccessToken, postData);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.SendEmail(apiAccessToken, postData);
            }
            return apiResult;
        }
        #endregion

        #region 发送微信模板消息
        /// <summary>
        /// 发送微信模板消息
        /// </summary>
        /// <param name="postData">
        /// 请求数据json格式
        /// {"ToOpenId":"{ToOpenId}","TemplateId":"{TemplateId}","Msg":"{Msg}","MsgUrl":"{SendMode}","SendMode":"{SendMode}","MsgColor":"{MsgColor}"}
        /// 选填:MsgUrl,SendMode,MsgColor
        /// 发送模式SendMode:now实时,day白天
        /// </param>
        /// <returns></returns>
        public static ApiResult SendTemplateMsg(string postData)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            var apiResult = UaApi.SendWeChat(apiAccessToken, postData);
            if (CheckRetryApiAccessToken(apiResult, out apiAccessToken))
            {
                apiResult = UaApi.SendWeChat(apiAccessToken, postData);
            }
            return apiResult;
        }
        #endregion

        #endregion

        #region 判断是否需要重新获取新ApiAccessToken
        /// <summary>
        /// 判断是否需要重新获取新ApiAccessToken
        /// </summary>
        /// <param name="apiResult"></param>
        /// <param name="newApiAccessToken"></param>
        /// <returns></returns>
        private static bool CheckRetryApiAccessToken(ApiResult apiResult, out string newApiAccessToken)
        {
            newApiAccessToken = "";
            if (apiResult.Status || !apiResult.Msg.Contains("app token is not latest"))
            {
                return false;
            }
            newApiAccessToken = ApiTokenHelper.GetApiAccessToken(true);
            return !string.IsNullOrEmpty(newApiAccessToken);
        }
        #endregion

        #region Jwt
        /// <summary>
        /// 校验Jwt Token
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public static ApiResult ValidateJwtToken(string jwtToken)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            return UaApi.ValidateJwtToken(apiAccessToken, jwtToken);
        }

        /// <summary>
        /// 刷新Jwt Token
        /// </summary>
        /// <param name="oldJwtToken"></param>
        /// <returns></returns>
        public static ApiResult RefreshJwtToken(string oldJwtToken)
        {
            var apiAccessToken = ApiTokenHelper.GetApiAccessToken();
            return UaApi.ValidateJwtToken(apiAccessToken, oldJwtToken);
        }
        #endregion
    }
}
