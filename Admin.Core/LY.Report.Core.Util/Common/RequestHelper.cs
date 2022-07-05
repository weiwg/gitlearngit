/* ******************************************************
 * 版权：广东易昂普软件信息有限公司
 * 作者：卢志成
 * 功能：请求帮助类
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20190124 luzhicheng  创建   
 ***************************************************** */

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace Eonup.Core.Util.Common
{
    /// <summary>
    /// 请求帮助类
    /// </summary>
    public class RequestHelper
    {
        #region URL请求数据
        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">POST的数据</param>
        /// <returns></returns>
        public static string HttpPost(string url, string param)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            string responseStr = null;

            try
            {
                //StreamWriter requestStream = new StreamWriter(request.GetRequestStream());
                //requestStream.Write(param);
                //requestStream.Close();

                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                responseStr = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }

            return responseStr;
        }

        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            string responseStr;

            try
            {
                WebResponse response = request.GetResponse();
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return responseStr;
        }

        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string WebClientGet(string url)
        {
            string response;
            try
            {
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                response = client.DownloadString(url);
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        /// <summary>
        /// 执行URL获取页面内容
        /// </summary>
        public static string UrlExecute(string urlPath)
        {
            if (string.IsNullOrEmpty(urlPath))
            {
                return "error";
            }
            StringWriter sw = new StringWriter();
            try
            {
                HttpContext.Current.Server.Execute(urlPath, sw);
                return sw.ToString();
            }
            catch (Exception)
            {
                return "error";
            }
            finally
            {
                sw.Dispose();
            }
        }
        #endregion

        #region 检测本机是否联网（互联网）
        [DllImport("wininet")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);
        /// <summary>
        /// 检测本机是否联网
        /// </summary>
        /// <returns>true已联网,false未联网</returns>
        public static bool IsConnectedInternet()
        {
            int i = 0;
            return InternetGetConnectedState(out i, 0);
        }
        #endregion

        #region 获取页面url
        /// <summary>
        /// 获取当前访问页面地址参数
        /// </summary>
        public static string GetScriptNameQueryString
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["QUERY_STRING"];
            }
        }
        /// <summary>
        /// 获取当前访问页面地址
        /// </summary>
        public static string GetScriptName
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];
            }
        }
        /// <summary>
        /// 获取当前访问页面Url
        /// </summary>
        public static string GetScriptUrl
        {
            get
            {
                return GetScriptNameQueryString == "" ? GetScriptName : string.Format("{0}?{1}", GetScriptName, GetScriptNameQueryString);
            }
        }
        /// <summary>
        ///  获取当前访问页面 参数
        /// </summary>
        public static string GetScriptNameQuery
        {
            get
            {
                return HttpContext.Current.Request.Url.Query;
            }
        }
        #endregion

        #region Url
        /// <summary>
        /// 得到当前完整主机名
        /// 网址：http://localhost:1897/News/Press/Content.aspx/123?id=1#toc 
        /// 结果：http://localhost:1897
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            try
            {
                HttpRequest request = HttpContext.Current.Request;
                return string.Format("{0}://{1}", request.Url.Scheme, request.Url.Authority);
            }
            catch (Exception)
            {
                // ignored
            }
            return "";
        }

        /// <summary>
        /// 得到当前主机名和端口号
        /// 网址：http://localhost:1897/News/Press/Content.aspx/123?id=1#toc 
        /// 结果：localhost:1897
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentHostPort()
        {
            try
            {
                return HttpContext.Current.Request.Url.Authority;
            }
            catch (Exception)
            {
                // ignored
            }
            return "";
        }

        /// <summary>
        /// 得到主机名
        /// 网址：http://localhost:1897/News/Press/Content.aspx/123?id=1#toc 
        /// 结果：localhost
        /// </summary>
        public static string GetCurrentHost()
        {
            try
            {
                return HttpContext.Current.Request.Url.Host;
            }
            catch (Exception)
            {
                // ignored
            }
            return "";
        }

        /// <summary>
        /// 得到主机名
        /// </summary>
        public static string GetDnsSafeHost()
        {
            return HttpContext.Current.Request.Url.DnsSafeHost;
        }

        /// <summary>
        /// 获得当前完整Url地址
        /// 网址：http://localhost:1897/News/Press/Content.aspx/123?id=1#toc 
        /// 结果：http://localhost:1897/News/Press/Content.aspx/123?id=1
        /// </summary>
        /// <returns>当前完整Url地址</returns>
        public static string GetCurrentUrl()
        {
            return HttpContext.Current.Request.Url.AbsoluteUri;
        }

        /// <summary>
        /// 获取当前请求的原始 URL(URL 中域信息之后的部分,包括查询字符串(如果存在))
        /// </summary>
        /// <returns>原始 URL</returns>
        public static string GetRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }

        /// <summary>
        /// 返回上一个页面的地址
        /// </summary>
        /// <returns>上一个页面的地址</returns>
        public static string GetUrlReferrer()
        {
            try
            {
                if (HttpContext.Current.Request.UrlReferrer != null)
                {
                    return HttpContext.Current.Request.UrlReferrer.ToString();
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return "";
        }

        #endregion

        #region URL处理
        /// <summary>
        /// URL字符编码
        /// </summary>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = str.Replace("'", "");
            return HttpContext.Current.Server.UrlEncode(str);
        }

        /// <summary>
        /// URL字符解码
        /// </summary>
        public static string UrlDecode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return HttpContext.Current.Server.UrlDecode(str);
        }
        #endregion

        #region 获得当前客户端的IP
        /// <summary>    
        /// 获得当前客户端的IP    
        /// </summary>    
        /// <returns>当前客户端的IP</returns>    
        public static string GetIpAddress()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result) || result == "::1")
            {
                return "127.0.0.1";
            }
            return result;
        }
        #endregion

        #region 获得当前客户端的浏览器
        /// <summary>    
        /// 获得当前客户端的浏览器
        /// </summary>    
        /// <returns>当前客户端的浏览器</returns>    
        public static string GetRequestBrowser()
        {
            return HttpContext.Current.Request.Browser.Browser; ;
        }
        #endregion

        #region 获得当前客户端的UserAgent
        /// <summary>    
        /// 获得当前客户端的UserAgent
        /// </summary>    
        /// <returns>当前客户端的UserAgent</returns>    
        public static string GetUserAgent()
        {
            return HttpContext.Current.Request.UserAgent; ;
        }
        #endregion

        #region 获得当前客户端的操作平台
        public static string GetPlatform()
        {
            string userAgent = GetUserAgent();
            int platformType = GetPlatform(userAgent);
            if (platformType == 11)
            {
                return "ComputerWeChat";
            }
            if (platformType == 21)
            {
                return "MobileWeChat";
            }
            if (platformType >= 10 && platformType < 20)
            {
                return "Computer";
            }
            if (platformType >= 20 && platformType < 30)
            {
                return "Mobile";
            }
            if (platformType == -1)
            {
                return "Other";
            }
            return "UnKnown";
        }
        /// <summary>
        /// 获得当前客户端的操作平台
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public static int GetPlatform(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
            {
                return -1;
            }
            userAgent = userAgent.ToLower();
            //手机系统
            string[] mobileOs =
            {
                "android", "iphone", "windows phone", "ipad", "blackberry", "meego", "symbianos"
            };
            if (mobileOs.Any(s => userAgent.IndexOf(s.ToLower(), StringComparison.Ordinal) > -1))
            {
                //微信
                if (userAgent.Contains("MicroMessenger".ToLower()))
                {
                    return 21;
                }
                return 20;
            }
            //PC端
            if (userAgent.Contains("Windows NT".ToLower()) || userAgent.Contains("Macintosh".ToLower()))
            {
                //微信
                if (userAgent.Contains("MicroMessenger".ToLower()))
                {
                    return 11;
                }
                return 10;
            }
            //其他设备(禁止访问)
            string[] forbidOs = { "Jodd HTTP", "mifetcher/1.0", "Baiduspider" };
            if (forbidOs.Any(s => userAgent.IndexOf(s.ToLower(), StringComparison.Ordinal) > -1))
            {
                return -1;
            }
            //其他未知
            return 0;
        }
        #endregion

        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns>是否接收到了Post请求</returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }

        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns>是否接收到了Get请求</returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }

        /// <summary>
        /// 返回指定的服务器变量信息
        /// </summary>
        /// <param name="strName">服务器变量名</param>
        /// <returns>服务器变量信息</returns>
        public static string GetServerString(string strName)
        {
            if (HttpContext.Current.Request.ServerVariables[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.ServerVariables[strName];
        }

        /// <summary>
        /// 判断当前访问是否来自浏览器软件
        /// </summary>
        /// <returns>当前访问是否来自浏览器软件</returns>
        public static bool IsBrowserGet()
        {
            string[] browserName = { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
            string curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
            for (int i = 0; i < browserName.Length; i++)
            {
                if (curBrowser.IndexOf(browserName[i], StringComparison.Ordinal) >= 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否来自搜索引擎链接
        /// </summary>
        /// <returns>是否来自搜索引擎链接</returns>
        public static bool IsSearchEnginesGet()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
                return false;

            string[] searchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
            string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
            for (int i = 0; i < searchEngine.Length; i++)
            {
                if (tmpReferrer.IndexOf(searchEngine[i], StringComparison.Ordinal) >= 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获得当前页面的名称
        /// </summary>
        /// <returns>当前页面的名称</returns>
        public static string GetPageName()
        {
            string[] urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return urlArr[urlArr.Length - 1].ToLower();
        }

        /// <summary>
        /// 返回表单或Url参数的总个数
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount()
        {
            return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
        }
    }
}
