using System;
using System.Web;

namespace EonUp.UnifiedAuth.Util.Tool
{
    public class CookieHelper
    {
        /// <summary>
        /// 判断cookies是否存在
        /// </summary>
        /// <param name="cookieName">名字</param>
        /// <returns></returns>
        public static bool IsNullCoolies(string cookieName)
        {
            return HttpContext.Current.Request.Cookies[cookieName] == null;
        }

        #region 清除
        /// <summary>  
        /// 清除指定Cookie  
        /// </summary>  
        /// <param name="cookieName">cookieName</param>  
        public static void ClearCookie(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-3);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
        #endregion

        #region 获取
        /// <summary>  
        /// 获取指定Cookie值  
        /// </summary>  
        /// <param name="cookieName">cookieName</param>  
        /// <returns></returns>  
        public static string GetCookieValue(string cookieName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            string str = string.Empty;
            if (cookie != null)
            {
                str = HttpUtility.UrlDecode(cookie.Value, System.Text.Encoding.UTF8);
            }
            return str;
        }
        #endregion

        #region 添加
        /// <summary>  
        /// 添加一个Cookie 默认当前会话有效
        /// </summary>  
        /// <param name="cookieName"></param>  
        /// <param name="cookieValue"></param>  
        /// <param name="expiresMinute">失效时间(分钟)</param>  
        public static void SetCookie(string cookieName, string cookieValue, int expiresMinute = 0)
        {
            SetCookie(cookieName, cookieValue, "", expiresMinute);
        }

        /// <summary>  
        /// 添加一个Cookie  
        /// </summary>  
        /// <param name="cookieName">cookie名</param>  
        /// <param name="cookieValue">cookie值</param>  
        /// <param name="cookieDomain">域名</param>
        /// <param name="expiresMinute">失效时间(分钟)</param>  
        public static void SetCookie(string cookieName, string cookieValue, string cookieDomain, int expiresMinute = 0)
        {
            DateTime expires = DateTime.MinValue;
            if (expiresMinute > 0)
            {
                expires = DateTime.Now.AddMinutes(expiresMinute);
            }
            SetCookie(cookieName, cookieValue, cookieDomain, "", expires);
        }

        /// <summary>  
        /// 添加一个Cookie  
        /// </summary>  
        /// <param name="cookieName">cookie名</param>  
        /// <param name="cookieValue">cookie值</param>  
        /// <param name="expires">过期时</param>  
        public static void SetCookie(string cookieName, string cookieValue, DateTime expires)
        {
            SetCookie(cookieName, cookieValue, "", "", expires);
        }

        /// <summary>
        /// 添加一个Cookie 默认过期时间24小时
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="cookieValue"></param>
        /// <param name="cookieDomain">域名</param>
        /// <param name="cookiePath">路径</param>
        /// <param name="expires">过期时间</param>
        public static void SetCookie(string cookieName, string cookieValue, string cookieDomain, string cookiePath, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            string value = HttpUtility.UrlEncode(cookieValue, System.Text.Encoding.UTF8);
            cookie.Value = string.IsNullOrEmpty(value) ? value : value.Replace("+", "%20");
            if (expires != DateTime.MinValue)
            {
                cookie.Expires = expires;
            }
            if (!string.IsNullOrEmpty(cookieDomain))
            {
                cookie.Domain = cookieDomain;
            }
            if (!string.IsNullOrEmpty(cookiePath))
            {
                cookie.Path = cookiePath;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        #endregion
    }
}
