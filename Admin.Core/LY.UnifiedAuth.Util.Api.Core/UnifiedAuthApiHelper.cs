/* ******************************************************
 * 作者：weig
 * 功能：API帮助类
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20191024 weigang  创建   
 ***************************************************** */

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using LY.UnifiedAuth.Util.Api.Core.Entity;
using LY.UnifiedAuth.Util.Api.Core.Util;

namespace LY.UnifiedAuth.Util.Api.Core
{
    /// <summary>
    /// Api帮助类
    /// </summary>
    public static class UnifiedAuthApiHelper
    {
        #region 生成调用Api的Url
        /// <summary>
        /// 生成调用Api的Url
        /// </summary>
        /// * 将apiUrl、timestamp、appSecret三个参数进行字典序排序(timestamp是Unix时间戳 UTC)
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 获得加密后的字符串可与signature对比，标识该请求来源正确。
        /// <param name="apiUrl">Api的Url</param>
        /// <param name="appId">AppId</param>
        /// <param name="appSecret">AppSecret</param>
        /// <param name="moreParams">更多参数</param>
        /// <returns></returns>
        public static string CreatApiSignatureUrl(string apiUrl, string appId, string appSecret, string moreParams = "")
        {
            if (string.IsNullOrEmpty(apiUrl) || string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(appSecret))
            {
                return "";
            }
            long timestamp = GetTimestamp();
            string signature = GetApiSignature(appId, appSecret, timestamp);
            string url =
                string.Format("{1}{0}appid={2}&timestamp={3}&signature={4}{5}",
                apiUrl.Contains("?") ? "&" : "?", apiUrl, appId, timestamp, signature,
                    string.IsNullOrEmpty(moreParams) ? "" : "&" + moreParams);
            return url;
        }
        #endregion

        #region 签名

        #region 验证Api签名
        /// <summary>
        /// 验证Api签名
        /// </summary>
        /// * 将AppId、timestamp、appSecret三个参数进行字典序排序(timestamp是Unix时间戳 UTC)
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源正确。
        /// <param name="appId">AppId</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="appSecret">AppSecret</param>
        /// <param name="signature">签名</param>
        /// <param name="effectiveTime">签名有效时间</param>
        /// <returns></returns>
        public static bool CheckApiSignature(string appId, string appSecret, long timestamp, string signature, int effectiveTime = 5)
        {
            if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(appSecret) || string.IsNullOrEmpty(signature) || timestamp <= 0)
            {
                return false;
            }

            //请求的Unix时间戳 与服务器时间差不能超过+-5分钟(默认)
            DateTime requestTime = GetDateTime(timestamp);
            if (requestTime.AddMinutes(-effectiveTime) > DateTime.Now || requestTime.AddMinutes(effectiveTime) < DateTime.Now)
            {
                return false;
            }
            //兼容旧方法
            if (signature == GetApiSignatureOld(appId, appSecret, timestamp))
            {
                return true;
            }
            return signature == GetApiSignature(appId, appSecret, timestamp);
        }

        /// <summary>
        /// 验证Api签名
        /// </summary>
        /// * 将AppId、timestamp、appSecret三个参数进行字典序排序(timestamp是Unix时间戳 UTC)
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源正确。
        /// <param name="parameters">签名参数</param>
        /// <param name="effectiveTime">签名有效时间</param>
        /// <returns></returns>
        public static bool CheckApiSignature(IDictionary<string, string> parameters, int effectiveTime = 5)
        {
            string signature = parameters["signature"];
            long timestamp = Convert.ToInt32(parameters["timestamp"]);
            if (string.IsNullOrEmpty(signature) || timestamp <= 0)
            {
                return false;
            }

            //请求的Unix时间戳 与服务器时间差不能超过+-5分钟(默认)
            DateTime requestTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().AddSeconds(timestamp);
            if (requestTime.AddMinutes(-effectiveTime) > DateTime.Now ||
                requestTime.AddMinutes(effectiveTime) < DateTime.Now)
            {
                return false;
            }
            parameters.Remove("signature");
            return signature == GetApiSignature(parameters);
        }
        #endregion
        
        #region 计算Api签名
        /// <summary>
        /// 计算Api签名
        /// </summary>
        /// <param name="appId">AppId</param>
        /// <param name="appSecret">AppSecret</param>
        /// <param name="timestamp">时间戳(Unix时间戳 UTC)</param>
        /// <returns></returns>
        public static string GetApiSignature(string appId, string appSecret, out long timestamp)
        {
            timestamp = GetTimestamp();
            return GetApiSignature(appId, appSecret, timestamp);
        }

        /// <summary>
        /// 计算Api签名
        /// </summary>
        /// <param name="appId">AppId</param>
        /// <param name="appSecret">AppSecret</param>
        /// <param name="timestamp">时间戳(Unix时间戳 UTC)</param>
        /// <returns></returns>
        private static string GetApiSignature(string appId, string appSecret, long timestamp)
        {
            Dictionary<string, string> sortedParams = new Dictionary<string, string>();
            sortedParams.Add("appid", appId);
            sortedParams.Add("appsecret", appSecret);
            sortedParams.Add("timestamp", timestamp.ToString());
            return GetApiSignature(sortedParams);
        }

        /// <summary>
        /// 计算Api签名
        /// </summary>
        /// <param name="appId">AppId</param>
        /// <param name="appSecret">AppSecret</param>
        /// <param name="timestamp">时间戳(Unix时间戳 UTC)</param>
        /// <returns></returns>
        private static string GetApiSignatureOld(string appId, string appSecret, long timestamp)
        {
            if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(appSecret) || timestamp <= 0)
            {
                return "";
            }

            string[] arrTmp = { appId, timestamp.ToString(), appSecret };
            Array.Sort(arrTmp); //字典排序
            string signature = string.Join("", arrTmp);
            return GetSignature(signature);
        }

        /// <summary>
        /// 计算Api签名
        /// </summary>
        /// <param name="parameters">签名参数</param>
        /// <returns></returns>
        private static string GetApiSignature(IDictionary<string, string> parameters)
        {
            if (string.IsNullOrEmpty(parameters["appid"]) || string.IsNullOrEmpty(parameters["appsecret"]) || Convert.ToInt32(parameters["timestamp"]) <= 0)
            {
                return "";
            }

            string signature = GetSignContent(parameters);
            return GetSignature(signature);
        }
        #endregion

        #region 验证Api数据签名
        /// <summary>
        /// 验证Api数据签名
        /// </summary>
        /// <param name="apiAccessToken">ApiAccessToken</param>
        /// <param name="timestamp">时间戳(Unix时间戳 UTC)</param>
        /// <param name="signature">签名</param>
        /// <param name="effectiveTime">签名有效时间</param>
        /// <returns></returns>
        public static bool CheckApiDataSignature(string apiAccessToken, long timestamp, string signature, int effectiveTime = 5)
        {
            if (string.IsNullOrEmpty(apiAccessToken) || string.IsNullOrEmpty(signature) || timestamp <= 0)
            {
                return false;
            }

            //请求的Unix时间戳 与服务器时间差不能超过+-5分钟(默认)
            DateTime requestTime = GetDateTime(timestamp);
            if (requestTime.AddMinutes(-effectiveTime) > DateTime.Now || requestTime.AddMinutes(effectiveTime) < DateTime.Now)
            {
                return false;
            }
            return signature == GetApiDataSignature(apiAccessToken, timestamp);
        }
        #endregion

        #region 计算Api数据签名
        /// <summary>
        /// 计算Api数据签名
        /// </summary>
        /// <param name="apiAccessToken">ApiAccessToken</param>
        /// <param name="timestamp">时间戳(Unix时间戳 UTC)</param>
        /// <returns></returns>
        public static string GetApiDataSignature(string apiAccessToken, out long timestamp)
        {
            timestamp = GetTimestamp();
            return GetApiDataSignature(apiAccessToken, timestamp);
        }

        /// <summary>
        /// 计算Api数据签名
        /// </summary>
        /// <param name="apiAccessToken">ApiAccessToken</param>
        /// <param name="timestamp">时间戳(Unix时间戳 UTC)</param>
        /// <returns></returns>
        public static string GetApiDataSignature(string apiAccessToken, long timestamp)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("key", "ApiData");
            parameters.Add("apiaccesstoken", apiAccessToken);
            parameters.Add("timestamp", timestamp.ToString());

            string signature = GetSignContent(parameters);
            return GetSignature(signature);
        }

        #endregion

        #region 签名方法
        /// <summary>
        /// 转换签名字符串
        /// </summary>
        /// <param name="parameters">签名参数</param>
        /// <returns></returns>
        private static string GetSignContent(IDictionary<string, string> parameters)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append("=").Append(value).Append("&");
                }
            }
            dem.Dispose();
            return query.ToString().Substring(0, query.Length - 1);
        }

        /// <summary>
        /// 计算签名
        /// </summary>
        /// <param name="paramStr">参数</param>
        /// <returns></returns>
        private static string GetSignature(string paramStr)
        {
            if (string.IsNullOrEmpty(paramStr))
            {
                return "";
            }
            string signature;
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(paramStr));
                signature = BitConverter.ToString(hash).Replace("-", "");
            }
            signature = string.IsNullOrEmpty(signature) ? "" : signature.ToLower();
            return signature;
        }

        #endregion

        #endregion

        #region AppAccessToken
        /// <summary>
        /// 创建AppAccessToken
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string CreateAppAccessToken(AppAccessToken token, string key)
        {
            if (string.IsNullOrEmpty(token.AppId) || string.IsNullOrEmpty(token.AppSecret) || token.ExpiresIn <= 0 || string.IsNullOrEmpty(key))
            {
                return "";
            }
            //string tokenJson = string.Format("{{\"AppId\": \"{0}\",\"AppSecret\": \"{1}\",\"ExpiresIn\": \"{2}\",\"ExpiresDate\": \"{3}\",\"Timestamp\":\"{4}\"}}", token.AppId, token.AppSecret, token.ExpiresIn, token.ExpiresDate, token.Timestamp);
            string tokenJson = string.Format("{{\"AppId\": \"{0}\",\"ExpiresDate\": \"{1}\",\"Timestamp\":\"{2}\"}}", token.AppId, token.ExpiresDate, token.Timestamp);
            return EncryptAes.Encrypt(tokenJson, key + "appat");
        }

        /// <summary>
        /// 解密AppAccessToken
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string DecryptAppAccessToken(string token, string key)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(key))
            {
                return "";
            }
            return EncryptAes.Decrypt(token, key + "appat");
        }

        /// <summary>
        /// 解密AppAccessToken
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static T DecryptAppAccessToken<T>(string token, string key)
        {
            return GetJsonObj<T>(DecryptAppAccessToken(token, key));
        }
        #endregion

        #region LoginToken
        /// <summary>
        /// 加密LoginToken
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="key">32字符秘钥</param>
        /// <returns></returns>
        public static string CreateLoginToken(AppLoginToken token, string key)
        {
            if (string.IsNullOrEmpty(token.UnifiedUserId) || string.IsNullOrEmpty(token.UserName) || string.IsNullOrEmpty(key))
            {
                return ""; 
            }
            string tokenJson = string.Format("{{\"UnifiedSessionId\":\"{0}\",\"UnifiedUserId\":\"{1}\",\"ApplyId\":\"{2}\",\"OpenId\":\"{3}\",\"UserName\":\"{4}\",\"NickName\":\"{5}\",\"LoginMode\":\"{6}\",\"Timestamp\":\"{7}\"}}", token.UnifiedSessionId, token.UnifiedUserId, token.ApplyId, token.OpenId, token.UserName, token.NickName, token.LoginMode, token.Timestamp);
            return EncryptAes.Encrypt(tokenJson, key + "logt");
        }

        /// <summary>
        /// 解密LoginToken
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="key">32字符秘钥</param>
        /// <returns></returns>
        public static string DecryptLoginToken(string token, string key)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(key))
            {
                return "";
            }
            return EncryptAes.Decrypt(token, key + "logt");
        }

        /// <summary>
        /// 解密LoginToken
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="key">32字符秘钥</param>
        /// <returns></returns>
        public static T DecryptLoginToken<T>(string token, string key)
        {
            return GetJsonObj<T>(DecryptLoginToken(token, key));
        }
        #endregion

        #region 加密/解密
        /// <summary>
        /// 加密内容
        /// </summary>
        /// <param name="text">明文字符串</param>
        /// <param name="appToken">appToken</param>
        /// <returns></returns>
        public static string Encrypt(string text, string appToken)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(appToken))
            {
                return "";
            }
            return EncryptAes.Encrypt(text, appToken);
        }

        /// <summary>
        /// 解密内容
        /// </summary>
        /// <param name="text">密文字符串</param>
        /// <param name="appToken">appToken</param>
        /// <returns></returns>
        public static string Decrypt(string text, string appToken)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(appToken))
            {
                return "";
            }
            return EncryptAes.Decrypt(text, appToken);
        }
        #endregion

        #region 时间戳
        /// <summary>
        /// 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
        /// </summary>
        /// <returns></returns>
        public static string GetTimestampStr()
        {
            return GetTimestamp().ToString();
        }

        /// <summary>
        /// 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
        /// </summary>
        /// <returns></returns>
        public static long GetTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }


        /// <summary>
        /// 根据时间戳获取时间，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        public static DateTime GetDateTime(long timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime().AddSeconds(timestamp);
        }

        /// <summary>
        /// 返回两个时间相差的秒数
        /// </summary>
        /// <param name="dt1">时间1</param>
        /// <param name="dt2">时间2</param>
        /// <returns></returns>
        public static int GetDateTimeTicks(DateTime dt1, DateTime dt2)
        {
            TimeSpan ts1 = new TimeSpan(dt1.Ticks);
            TimeSpan ts2 = new TimeSpan(dt2.Ticks);
            return Convert.ToInt32(ts1.Subtract(ts2).Duration().TotalSeconds);
        }
        #endregion

        #region Json处理数据
        /// <summary>
        /// obj to json string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetJsonStr(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception e)
            {
                ApiLogHelper.Error(e, $"GetJsonStr");
                return "";
            }
        }

        /// <summary>
        /// json string to obj
        /// </summary>
        /// <param name="json"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetJsonObj<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return Activator.CreateInstance<T>();
            }
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                ApiLogHelper.Error(e, $"GetJsonObj");
                return Activator.CreateInstance<T>();
            }
        }
        #endregion
    }
}