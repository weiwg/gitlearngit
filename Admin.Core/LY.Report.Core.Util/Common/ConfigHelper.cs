﻿using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace LY.Report.Core.Util.Common
{
    /// <summary>
    /// 配置帮助类
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="environmentName">环境名称</param>
        /// <param name="reloadOnChange">自动更新</param>
        /// <returns></returns>
        public static IConfiguration Load(string fileName, string environmentName = "", bool reloadOnChange = false)
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "configs");
            if (!Directory.Exists(filePath))
                return null;

            var builder = new ConfigurationBuilder()
                .SetBasePath(filePath)
                .AddJsonFile(fileName.ToLower() + ".json", true, reloadOnChange);

            if (!string.IsNullOrEmpty(environmentName))
            {
                builder.AddJsonFile(fileName.ToLower() + "." + environmentName + ".json", optional: true, reloadOnChange: reloadOnChange);
            }

            return builder.Build();
        }

        /// <summary>
        /// 获得配置信息
        /// </summary>
        /// <typeparam name="T">配置信息</typeparam>
        /// <param name="fileName">文件名称</param>
        /// <param name="environmentName">环境名称</param>
        /// <param name="reloadOnChange">自动更新</param>
        /// <returns></returns>
        public static T Get<T>(string fileName, string environmentName = "", bool reloadOnChange = false)
        {
            var config = Load(fileName, environmentName, reloadOnChange);
            if (config == null)
                return default;

            return config.Get<T>();
        }

        /// <summary>
        /// 获得配置信息
        /// </summary>
        /// <typeparam name="T">配置信息</typeparam>
        /// <param name="environmentName">环境名称</param>
        /// <param name="reloadOnChange">自动更新</param>
        /// <returns></returns>
        public static T GetAppConfig<T>(string environmentName = "", bool reloadOnChange = false)
        {
            var config = Load("appconfig", environmentName, reloadOnChange);
            if (config == null)
                return default;

            return config.Get<T>();
        }

        /// <summary>
        /// 绑定实例配置信息
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="instance">实例配置</param>
        /// <param name="environmentName">环境名称</param>
        /// <param name="reloadOnChange">自动更新</param>
        public static void Bind(string fileName, object instance, string environmentName = "", bool reloadOnChange = false)
        {
            var config = Load(fileName, environmentName, reloadOnChange);
            if (config == null || instance == null)
                return;

            config.Bind(instance);
        }
    }
}
