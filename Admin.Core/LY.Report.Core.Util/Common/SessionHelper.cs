using System;
using System.Web;

namespace Eonup.Core.Util.Common
{
    /// <summary>
    /// Session帮助类
    /// </summary>
    public class SessionHelper
    {
        private const string GlobalSessionName = "Mall";

        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetStr(string name)
        {
            return Convert.ToString(Get(name));
        }

        /// <summary>
        /// 获取Session
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object Get(string name)
        {
            object value = HttpContext.Current.Session[GlobalSessionName + "_" + name];
            if (string.IsNullOrEmpty(name) || value == null)
            {
                return "";
            }
            return value;
        }

        /// <summary>
        /// 获取Session并转换为实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T Get<T>(string name)
        {
            object value = HttpContext.Current.Session[GlobalSessionName + "_" + name];
            if (string.IsNullOrEmpty(name) || value == null)
            {
                return Activator.CreateInstance<T>();
            }
            try
            {
                return (T)value;
            }
            catch (Exception)
            {
                return Activator.CreateInstance<T>();
            }
        }

        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void Set(string name,object value)
        {
            HttpContext.Current.Session[GlobalSessionName + "_" + name] = value;
        }

        /// <summary>
        /// 移除指定Session信息
        /// </summary>
        public static void Remove(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            HttpContext.Current.Session.Remove(GlobalSessionName + "_" + name);
        }

        /// <summary>
        /// 清空所有Session对象的值，但保留会话
        /// </summary>
        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }
    }

}
