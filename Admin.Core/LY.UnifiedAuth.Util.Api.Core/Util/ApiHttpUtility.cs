/* ******************************************************
 * 作者：weig
 * 功能：Http请求
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20191024 weigang  创建   
 ***************************************************** */

using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LY.UnifiedAuth.Util.Api.Core.Util
{
    /// <summary>
    /// Http请求
    /// </summary>
    public class ApiHttpUtility
    {
        /// <summary>
        /// 请求超时设置（以毫秒为单位），默认为10秒。
        /// 说明：此处常量专为提供给方法的参数的默认值，不是方法内所有请求的默认超时时间。
        /// </summary>
        private const int TimeOut = 10000;
        /// <summary>
        /// 默认代理
        /// </summary>
        private const string UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.438 EupUaHelper/1.0 (Core)";

        #region Get

        #region 同步
        /// <summary>
        /// Get请求（没有加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string HttpGet(string url, Hashtable headers, Encoding encoding)
        {
            ApiLogHelper.Debug($"HttpGet > Url:{url}");
            var result = WebRequestGet(url, headers, encoding);
            ApiLogHelper.Debug($"HttpGet > Result:{result}");
            return result;
        }

        /// <summary>
        /// Get请求（加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="encoding"></param>
        /// <param name="timeOut">请求超时设置（以毫秒为单位），默认为10秒。</param>
        /// <returns></returns>
        public static string HttpGet(string url, Hashtable headers, CookieContainer cookieContainer = null, Encoding encoding = null, int timeOut = TimeOut)
        {
            ApiLogHelper.Debug($"HttpGet > Url:{url}");
            var result = WebRequestGet(url, headers, cookieContainer, encoding, timeOut);
            ApiLogHelper.Debug($"HttpGet > Result:{result}");
            return result;
        }
        #endregion

        #region 异步
        /// <summary>
        /// Get请求（没有加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static async Task<string> HttpGetAsync(string url, Hashtable headers, Encoding encoding)
        {
            ApiLogHelper.Debug($"HttpGetAsync > Url:{url}");
            var result = await WebRequestGetAsync(url, headers, encoding);
            ApiLogHelper.Debug($"HttpGetAsync > Result:{result}");
            return result;
        }

        /// <summary>
        /// Get请求（加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="encoding"></param>
        /// <param name="timeOut">请求超时设置（以毫秒为单位），默认为10秒。</param>
        /// <returns></returns>
        public static async Task<string> HttpGetAsync(string url, Hashtable headers, CookieContainer cookieContainer = null, Encoding encoding = null, int timeOut = TimeOut)
        {
            ApiLogHelper.Debug($"HttpGetAsync > Url:{url}");
            var result = await WebRequestGetAsync(url, headers, cookieContainer, encoding, timeOut);
            ApiLogHelper.Debug($"HttpGetAsync > Result:{result}");
            return result;
        }
        #endregion
        
        #endregion

        #region Post
        
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="postData">data</param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string HttpPost(string url, string postData, Hashtable headers)
        {
            ApiLogHelper.Debug($"HttpPost > Url:{url},Data:{postData}");
            var result = WebRequest(url, postData, headers, "POST");
            ApiLogHelper.Debug($"HttpPost > Result:{result}");
            return result;
        }

        /// <summary>
        /// Put请求
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="postData">data</param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<string> HttpPostAsync(string url, string postData, Hashtable headers)
        {
            ApiLogHelper.Debug($"HttpPostAsync > Url:{url},Data:{postData}");
            var result = await WebRequestAsync(url, postData, headers, "POST");
            ApiLogHelper.Debug($"HttpPostAsync > Result:{result}");
            return result;
        }

        #endregion

        #region Put
        
        /// <summary>
        /// Put请求
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="putData">data</param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string HttpPut(string url, string putData, Hashtable headers)
        {
            ApiLogHelper.Debug($"HttpPut > Url:{url},Data:{putData}");
            var result = WebRequest(url, putData, headers, "PUT");
            ApiLogHelper.Debug($"HttpPut > Result:{result}");
            return result;
        }

        /// <summary>
        /// Put请求
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="putData">data</param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<string> HttpPutAsync(string url, string putData, Hashtable headers)
        {
            ApiLogHelper.Debug($"HttpPutAsync > Url:{url},Data:{putData}");
            var result = await WebRequestAsync(url, putData, headers, "PUT");
            ApiLogHelper.Debug($"HttpPutAsync > Result:{result}");
            return result;
        }

        #endregion

        #region Delete

        /// <summary>
        /// Delete请求
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="deleteData">data</param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string HttpDelete(string url, string deleteData, Hashtable headers)
        {
            ApiLogHelper.Debug($"DeleteApiResult > Url:{url},Data:{deleteData}");
            var result = WebRequest(url, deleteData, headers, "DELETE");
            ApiLogHelper.Debug($"DeleteApiResult > Result:{result}");
            return result;
        }

        /// <summary>
        /// Delete请求
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="deleteData">data</param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<string> HttpDeleteAsync(string url, string deleteData, Hashtable headers)
        {
            ApiLogHelper.Debug($"HttpDeleteAsync > Url:{url},Data:{deleteData}");
            var result = await WebRequestAsync(url, deleteData, headers, "DELETE");
            ApiLogHelper.Debug($"HttpDeleteAsync > Result:{result}");
            return result;
        }

        #endregion

        #region Request请求

        #region Get

        #region 同步
        /// <summary>
        /// Get请求（没有加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private static string WebRequestGet(string url, Hashtable headers, Encoding encoding)
        {
            try
            {
                WebClientPro wc = new WebClientPro();
                wc.Timeout = TimeOut;
                wc.Encoding = encoding ?? Encoding.UTF8;
                if (headers != null)
                {
                    foreach (DictionaryEntry de in headers)
                    {
                        if (!string.IsNullOrEmpty(de.Value.ToString()))
                        {
                            wc.Headers[de.Key.ToString()] = de.Value.ToString();
                        }
                    }
                }
                wc.Headers["User-Agent"] = UserAgent;
                return wc.DownloadString(url);
            }
            catch (Exception e)
            {
                ApiLogHelper.Error(e, $"HttpGet Error ,Msg:{e.Message},Url:{url}");
                throw;
            }
        }

        /// <summary>
        /// Get请求（加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="encoding"></param>
        /// <param name="timeOut">请求超时设置（以毫秒为单位），默认为10秒。</param>
        /// <returns></returns>
        private static string WebRequestGet(string url, Hashtable headers, CookieContainer cookieContainer = null, Encoding encoding = null, int timeOut = TimeOut)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = timeOut;

                if (headers != null)
                {
                    foreach (DictionaryEntry de in headers)
                    {
                        if (!string.IsNullOrEmpty(de.Value.ToString()))
                        {
                            request.Headers[de.Key.ToString()] = de.Value.ToString();
                        }
                    }
                }
                request.UserAgent = UserAgent;

                if (cookieContainer != null)
                {
                    request.CookieContainer = cookieContainer;
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (cookieContainer != null)
                {
                    response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
                }

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream is null)
                    {
                        return "";
                    }
                    using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.UTF8))
                    {
                        return myStreamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                ApiLogHelper.Error(e, $"HttpGet Error ,Msg:{e.Message},Url:{url}");
                throw;
            }
        }
        #endregion

        #region 异步
        /// <summary>
        /// Get请求（没有加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private static async Task<string> WebRequestGetAsync(string url, Hashtable headers, Encoding encoding)
        {
            try
            {
                WebClientPro wc = new WebClientPro();
                wc.Timeout = TimeOut;
                wc.Encoding = encoding ?? Encoding.UTF8;
                if (headers != null)
                {
                    foreach (DictionaryEntry de in headers)
                    {
                        if (!string.IsNullOrEmpty(de.Value.ToString()))
                        {
                            wc.Headers[de.Key.ToString()] = de.Value.ToString();
                        }
                    }
                }
                wc.Headers["User-Agent"] = UserAgent;
                return await wc.DownloadStringTaskAsync(url);
            }
            catch (Exception e)
            {
                ApiLogHelper.Error(e, $"HttpGet Error ,Msg:{e.Message},Url:{url}");
                throw;
            }
        }

        /// <summary>
        /// Get请求（加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="encoding"></param>
        /// <param name="timeOut">请求超时设置（以毫秒为单位），默认为10秒。</param>
        /// <returns></returns>
        private static async Task<string> WebRequestGetAsync(string url, Hashtable headers, CookieContainer cookieContainer = null, Encoding encoding = null, int timeOut = TimeOut)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout = timeOut;

                if (headers != null)
                {
                    foreach (DictionaryEntry de in headers)
                    {
                        if (!string.IsNullOrEmpty(de.Value.ToString()))
                        {
                            request.Headers[de.Key.ToString()] = de.Value.ToString();
                        }
                    }
                }
                request.UserAgent = UserAgent;

                if (cookieContainer != null)
                {
                    request.CookieContainer = cookieContainer;
                }

                HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());

                if (cookieContainer != null)
                {
                    response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
                }

                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream is null)
                    {
                        return "";
                    }
                    using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.UTF8))
                    {
                        return await myStreamReader.ReadToEndAsync();
                    }
                }
            }
            catch (Exception e)
            {
                ApiLogHelper.Error(e, $"HttpGet Error ,Msg:{e.Message},Url:{url}");
                throw;
            }
        }
        #endregion

        #endregion

        #region Post/Put/Delete

        #region 同步
        /// <summary>
        /// Request请求
        /// </summary>
        /// <param name="postData">data</param>
        /// <param name="url">url</param>
        /// <param name="headers">headers</param>
        /// <param name="method">method</param>
        /// <returns></returns>
        private static string WebRequest(string url, string postData, Hashtable headers, string method)
        {
            byte[] data = Encoding.UTF8.GetBytes(postData);

            HttpWebRequest request = (HttpWebRequest) System.Net.WebRequest.Create(url);
            request.Method = string.IsNullOrEmpty(method) ? "POST" : method;
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.KeepAlive = true;
            request.UserAgent = UserAgent;

            if (headers != null)
            {
                foreach (DictionaryEntry de in headers)
                {
                    if (!string.IsNullOrEmpty(de.Value.ToString()))
                    {
                        request.Headers[de.Key.ToString()] = de.Value.ToString();
                    }
                }
            }

            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream is null)
                    {
                        return "";
                    }
                    using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (WebException e)
            {
                ApiLogHelper.Error(e, $"HttpPost Error ,Msg:{e.Message},Url:{url},Data:{postData}");
                throw;
            }
            finally
            {
                request.Abort();
                response?.Close();
                stream.Close();
                stream.Dispose();
            }
        }
        #endregion

        #region 异步
        /// <summary>
        /// Request请求
        /// </summary>
        /// <param name="postData">data</param>
        /// <param name="url">url</param>
        /// <param name="headers">headers</param>
        /// <param name="method">method</param>
        /// <returns></returns>
        private static async Task<string> WebRequestAsync(string url, string postData, Hashtable headers, string method)
        {
            byte[] data = Encoding.UTF8.GetBytes(postData);

            HttpWebRequest request = (HttpWebRequest) System.Net.WebRequest.Create(url);
            request.Method = string.IsNullOrEmpty(method) ? "POST" : method;
            request.ContentType = "application/json";
            request.ContentLength = data.Length;
            request.KeepAlive = true;
            request.UserAgent = UserAgent;

            if (headers != null)
            {
                foreach (DictionaryEntry de in headers)
                {
                    if (!string.IsNullOrEmpty(de.Value.ToString()))
                    {
                        request.Headers[de.Key.ToString()] = de.Value.ToString();
                    }
                }
            }

            Stream stream = request.GetRequestStream();
            await stream.WriteAsync(data, 0, data.Length);
            WebResponse response = null;
            try
            {
                response = await request.GetResponseAsync();
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream is null)
                    {
                        return "";
                    }
                    using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
            }
            catch (WebException e)
            {
                ApiLogHelper.Error(e, $"HttpPost Error ,Msg:{e.Message},Url:{url},Data:{postData}");
                throw;
            }
            finally
            {
                request.Abort();
                response?.Close();
                stream.Close();
                stream.Dispose();
            }

        }
        #endregion

        #endregion

        #endregion
    }
}
