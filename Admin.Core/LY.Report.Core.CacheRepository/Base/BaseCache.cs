using System.Text;

namespace LY.Report.Core.CacheRepository.Base
{
    /// <summary>
    /// 数据缓存
    /// </summary>
    public class BaseCache
    {
        //全局默认缓存时间(秒)(2小时)
        public static int CacheExpiresTime => GlobalConfig.CacheExpiresTime;

        /// <summary>
        /// 获取缓存的key值
        /// </summary>
        /// <param name="business"></param>
        /// <returns></returns>
        public static string GetCacheKeyName(string business)
        {
            return GetCacheKeyName(business, "");
        }

        /// <summary>
        /// 获取缓存的key值
        /// </summary>
        /// <param name="business"></param>
        /// <param name="keyArr"></param>
        /// <returns></returns>
        public static string GetCacheKeyName(string business, string[] keyArr)
        {
            StringBuilder keySb = new StringBuilder();
            foreach (string key in keyArr)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    keySb.Append(string.Format("{0}:", key));
                }
            }
            return GetCacheKeyName("Mall", business, keySb.Length > 0 ? keySb.ToString().Substring(0, keySb.Length - 1) : "");
        }

        /// <summary>
        /// 获取缓存的key值
        /// </summary>
        /// <param name="business"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCacheKeyName(string business, string key)
        {
            return GetCacheKeyName("Mall", business, key);
        }

        /// <summary>
        /// 获取缓存的key值
        /// </summary>
        /// <param name="system"></param>
        /// <param name="business"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetCacheKeyName(string system, string business, string key)
        {
            return string.Format("{0}:{1}:{2}", system, string.IsNullOrEmpty(business) ? "All" : business, string.IsNullOrEmpty(key) ? "All": key);
        }
    }
}
