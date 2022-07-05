using System.Collections.Generic;
using LY.Report.Core.CacheRepository.Base;
using LY.Report.Core.Common.Cache;
using LY.Report.Core.Model.System;
using LY.Report.Core.Repository.System;
using LY.Report.Core.Util.Middleware;

namespace LY.Report.Core.CacheRepository
{
    public class GlobalHelper : BaseCache
    {
        private static T GetService<T>() where T : class
        {
            return HttpService.GetService<T>();
        }

        #region 网站配置

        #region 获取当前WebConfig
        private static readonly object LockerInsertWebConfigObj = new object();
        /// <summary>
        /// 获取WebConfig
        /// </summary>
        /// <param name="isRefresh">是否强制刷新</param>
        /// <returns></returns>
        public static SysWebConfig GetWebConfig(bool isRefresh = false)
        {
            var webConfig = new SysWebConfig();
            if (!GetService<ICache>().Exists("Mall_WebConfig") || isRefresh)
            {
                lock (LockerInsertWebConfigObj)
                {

                    var data = GetService<ISysWebConfigRepository>()
                        .GetOneAsync<SysWebConfig>(t => true).Result;
                    if (data != null)
                    {
                        webConfig = data;
                        GetService<ICache>().Set("Mall_WebConfig", webConfig, 86400); //存储24小时
                    }
                }
            }
            else
            {
                webConfig = GetService<ICache>().Get<SysWebConfig>("Mall_WebConfig");
            }
            return webConfig;
        }
        #endregion

        #endregion

        #region 系统参数配置
        /// <summary>
        /// 获取系统参数配置
        /// </summary>
        /// <param name="paramKey"></param>
        /// <param name="isRefresh">是否强制刷新</param>
        /// <returns></returns>
        public static string GetParamConfigValue(string paramKey, bool isRefresh = false)
        {
            return GetParamConfig(paramKey, isRefresh).ParamValue;
        }

        /// <summary>
        /// 获取系统参数配置
        /// </summary>
        /// <param name="paramKey"></param>
        /// <param name="isRefresh">是否强制刷新</param>
        /// <returns></returns>
        public static SysParamConfig GetParamConfig(string paramKey, bool isRefresh = false)
        {
            SysParamConfig paramConfig = null;
            List<SysParamConfig> paramConfigList = GetAllParamConfig();
            //缓存中存在数据
            if (paramConfigList != null && paramConfigList.Count > 0 && !isRefresh)
            {
                paramConfig = paramConfigList.Find(t => t.ParamKey == paramKey);
            }
            //缓存没有数据就读数据库的数据并更新缓存
            if (isRefresh || paramConfig == null || string.IsNullOrEmpty(paramConfig.Id))
            {
                paramConfig = ResetParamConfig(paramKey);
            }
            return paramConfig ?? new SysParamConfig();
        }

        #region 获取全部参数配置
        /// <summary>
        /// 获取缓存的所有系统参数配置
        /// </summary>
        /// <returns></returns>
        public static List<SysParamConfig> GetAllParamConfig()
        {
            var paramConfigList = new List<SysParamConfig>();
            if (GetService<ICache>().Exists("Mall_ParamConfig"))
            {
                paramConfigList = GetService<ICache>().Get<List<SysParamConfig>>("Mall_ParamConfig");
            }
            return paramConfigList;
        }
        #endregion

        #region 重设参数配置
        private static readonly object LockerInsertParamConfigObj = new object();

        /// <summary>
        /// 重设单个缓存数据
        /// </summary>
        /// <param name="paramKey"></param>
        /// <returns></returns>
        public static SysParamConfig ResetParamConfig(string paramKey)
        {
            var paramConfig = new SysParamConfig();
            if (!string.IsNullOrEmpty(paramKey))
            {
                return paramConfig;
            }
            lock (LockerInsertParamConfigObj)
            {
                var data = GetService<ISysParamConfigRepository>()
                    .GetOneAsync<SysParamConfig>(t=>t.ParamKey == paramKey).Result;
                if (data != null)
                {
                    paramConfig = data;
                }
                //获取当前信息
                List<SysParamConfig> paramConfigList = GetAllParamConfig();
                paramConfigList.RemoveAll(t => t.ParamKey == paramKey);
                if (!string.IsNullOrEmpty(paramConfig.ParamKey))
                {
                    paramConfigList.Add(paramConfig);//保存新增的
                }
                GetService<ICache>().Set("Mall_ParamConfig", paramConfigList, 86400); //存储24小时
            }
            return paramConfig;
        }

        private static readonly object LockerInsertAllParamConfigObj = new object();

        /// <summary>
        /// 重设缓存数据
        /// </summary>
        /// <returns></returns>
        public static List<SysParamConfig> ResetAllParamConfig()
        {
            List<SysParamConfig> paramConfigList = new List<SysParamConfig>();
            lock (LockerInsertAllParamConfigObj)
            {
                var data = GetService<ISysParamConfigRepository>()
                    .GetListAsync<SysParamConfig>(t => true).Result;
                if (data != null)
                {
                    paramConfigList = data;
                    GetService<ICache>().Set("Mall_ParamConfig", paramConfigList, 86400); //存储24小时
                }
            }
            return paramConfigList;
        }
        #endregion
        #endregion
        
    }
}
