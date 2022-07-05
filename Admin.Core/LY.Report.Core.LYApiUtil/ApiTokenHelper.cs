using System;
using System.Collections.Generic;
using LY.Report.Core.CacheRepository.Base;
using LY.Report.Core.Common.Cache;
using LY.UnifiedAuth.Util.Api.Core.Entity;
using LY.Report.Core.CacheRepository;
using LY.Report.Core.LYApiUtil.Ua;
using LY.Report.Core.Util.Middleware;

namespace LY.Report.Core.LYApiUtil
{
    public class ApiTokenHelper : BaseCache
    {
        private static T GetService<T>() where T : class
        {
            return HttpService.GetService<T>();
        }

        #region Api令牌
        /// <summary>
        /// 获取Api令牌
        /// </summary>
        /// <param name="isRefresh">是否强制更新数据</param>
        /// <returns></returns>
        public static string GetApiAccessToken(bool isRefresh = false)
        {
            return GetApiAccessTokenEntity(isRefresh).AccessToken;
        }

        /// <summary>
        /// 获取Api系统认证令牌
        /// </summary>
        /// <param name="isRefresh">是否强制更新数据</param>
        /// <returns></returns>
        public static string GetApiSysAuthToken(bool isRefresh = false)
        {
            return GetApiAccessTokenEntity(isRefresh).SysAuthToken;
        }

        #region ApiAccessToken
        /// <summary>
        /// 获取Api令牌
        /// </summary>
        /// <param name="isRefresh">是否强制更新数据</param>
        /// <returns></returns>
        public static AppAccessToken GetApiAccessTokenEntity(bool isRefresh = false)
        {
            var apiAccessToken = new AppAccessToken();
            if (isRefresh)
            {
                apiAccessToken = ResetApiAccessTokenCache();
            }
            else
            {
                var apiAccessTokenList = GetAllApiAccessToken();
                if (apiAccessTokenList != null && apiAccessTokenList.Count > 0)
                {
                    apiAccessToken = apiAccessTokenList.Find(t => t.AppId == GlobalConfig.LYAppId);
                }
            }
            if (apiAccessToken == null || string.IsNullOrEmpty(apiAccessToken.AccessToken) || apiAccessToken.ExpiresDate < DateTime.Now)
            {
                apiAccessToken = ResetApiAccessTokenCache();
            }
            return apiAccessToken ?? new AppAccessToken();
        }
        #endregion

        #region 获取全部App令牌
        /// <summary>
        /// 获取全部App令牌
        /// </summary>
        /// <returns></returns>
        public static List<AppAccessToken> GetAllApiAccessToken()
        {
            var authAccessTokenList = new List<AppAccessToken>();
            if (GetService<ICache>().Exists("All_Api_AccessToken"))
            {
                authAccessTokenList = GetService<ICache>().Get<List<AppAccessToken>>("All_Api_AccessToken");
            }
            return authAccessTokenList;
        }
        #endregion

        #region 重设当前App令牌缓存
        private static readonly object LockerReApiAccessTokenObj = new object();
        /// <summary>
        /// 重设当前App令牌缓存
        /// </summary>
        public static AppAccessToken ResetApiAccessTokenCache()
        {
            var apiAccessToken = new AppAccessToken();
            lock (LockerReApiAccessTokenObj)
            {
                bool isDebug = false;
                #if DEBUG
                isDebug = true;
                #endif
                var apiResult = UaApiHelper.GetApiAccessToken(isDebug);
                if (apiResult.Status || apiResult.Data != null)
                {
                    var tokenHt = apiResult.Data;
                    apiAccessToken.AppId = GlobalConfig.LYAppId;
                    apiAccessToken.AccessToken = Convert.ToString(tokenHt["AccessToken"]);
                    apiAccessToken.SysAuthToken = Convert.ToString(tokenHt["SysAuthToken"]);
                    apiAccessToken.SysAuthJwtToken = Convert.ToString(tokenHt["SysAuthJwtToken"]);
                    apiAccessToken.ExpiresIn = Convert.ToInt32(tokenHt["ExpiresIn"]);
                    apiAccessToken.ExpiresIn = apiAccessToken.ExpiresIn > 60 ? apiAccessToken.ExpiresIn - 60 : apiAccessToken.ExpiresIn;//提前一分钟超时
                    apiAccessToken.ExpiresDate = DateTime.Now.AddSeconds(apiAccessToken.ExpiresIn);
                }
                //获取当前信息
                var apiAccessTokenList = GetAllApiAccessToken();
                apiAccessTokenList.RemoveAll(t => t.ExpiresDate < DateTime.Now || t.AppId == apiAccessToken.AppId);//剔除失效的
                if (!string.IsNullOrEmpty(apiAccessToken.AccessToken))
                {
                    apiAccessTokenList.Add(apiAccessToken);//保存新增的
                }
                GetService<ICache>().Set("All_Api_AccessToken", apiAccessTokenList, apiAccessToken.ExpiresIn);
            }
            return apiAccessToken;
        }
        #endregion

        #endregion

        #region 校验系统认证令牌
        /// <summary>
        /// 校验系统认证令牌
        /// </summary>
        /// <param name="authToken">系统认证令牌</param>
        /// <param name="expiresDate">过期时间</param>
        /// <returns></returns>
        public static bool CheckAuthToken(string authToken, out DateTime expiresDate)
        {
            var authAccessToken = GetAppAuthToken(authToken);
            expiresDate = authAccessToken.ExpiresDate;
            if (string.IsNullOrEmpty(authAccessToken.SysAuthToken))
            {
                return false;
            }
            if (authAccessToken.ExpiresDate.AddMinutes(5) < DateTime.Now)
            {
                ResetAppAuthTokenCache("");//移除已失效token
                return false;
            }
            return true;
        }

        #region AuthAccessToken
        /// <summary>
        /// 获取当前AuthAccessToken
        /// </summary>
        /// <param name="authToken">系统认证令牌</param>
        /// <returns></returns>
        public static AppAccessToken GetAppAuthToken(string authToken)
        {
            var authAccessToken = new AppAccessToken();
            var authAccessTokenList = GetAllAppAuthToken();
            if (authAccessTokenList != null && authAccessTokenList.Count > 0)
            {
                authAccessToken = authAccessTokenList.Find(t => t.SysAuthToken == authToken);
            }
            if (authAccessToken == null || string.IsNullOrEmpty(authAccessToken.SysAuthToken))
            {
                authAccessToken = ResetAppAuthTokenCache(authToken);
            }
            return authAccessToken ?? new AppAccessToken();
        }
        #endregion

        #region 获取全部App令牌
        /// <summary>
        /// 获取全部App令牌
        /// </summary>
        /// <returns></returns>
        public static List<AppAccessToken> GetAllAppAuthToken()
        {
            var authAccessTokenList = new List<AppAccessToken>();
            if (GetService<ICache>().Exists("All_Auth_AccessToken"))
            {
                authAccessTokenList = GetService<ICache>().Get<List<AppAccessToken>>("All_Auth_AccessToken");
            }
            return authAccessTokenList;
        }
        #endregion

        #region 重设当前AuthAccessToken缓存
        private static readonly object LockerReAppAuthTokenObj = new object();
        /// <summary>
        /// 重设当前AuthAccessToken缓存
        /// </summary>
        /// <param name="authToken">系统认证令牌</param>
        public static AppAccessToken ResetAppAuthTokenCache(string authToken)
        {
            var authAccessToken = new AppAccessToken();
            lock (LockerReAppAuthTokenObj)
            {
                if (!string.IsNullOrEmpty(authToken))
                {
                    var apiResult = UaApiHelper.CheckAuthToken(authToken);
                    if (apiResult.Status || apiResult.Data != null)
                    {
                        var tokenHt = apiResult.Data;
                        authAccessToken.AppId = Convert.ToString(tokenHt["AppId"]);
                        authAccessToken.SysAuthToken = authToken;
                        authAccessToken.ExpiresIn = Convert.ToInt32(tokenHt["ExpiresIn"]);
                        authAccessToken.ExpiresDate = DateTime.Now.AddSeconds(authAccessToken.ExpiresIn);
                    }
                }

                //获取当前信息
                var authAccessTokenList = GetAllAppAuthToken();
                authAccessTokenList.RemoveAll(t => t.ExpiresDate < DateTime.Now);//剔除失效的
                if (!string.IsNullOrEmpty(authAccessToken.SysAuthToken))
                {
                    authAccessTokenList.RemoveAll(t => t.SysAuthToken == authToken);//先剔除重复的
                    authAccessTokenList.Add(authAccessToken);//保存新增的
                }

                GetService<ICache>().Set("All_Auth_AccessToken", authAccessTokenList, 7200); //存储2小时
            }
            return authAccessToken;
        }
        #endregion

        #endregion

    }
}
