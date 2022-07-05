using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web;

namespace LY.Report.Core.Util.Common
{
    /// <summary>
    /// Url的操作类
    /// </summary>
    public class UrlExtendHelper
    {
        private static readonly System.Text.Encoding Encoding = System.Text.Encoding.UTF8;

        #region URL的64位编码
        public static string Base64Encrypt(string sourthUrl)
        {
            string eurl = HttpUtility.UrlEncode(sourthUrl);
            if (eurl != null)
            {
                eurl = Convert.ToBase64String(Encoding.GetBytes(eurl));
                return eurl;
            }
            return "";
        }
        #endregion

        #region URL的64位解码
        public static string Base64Decrypt(string eStr)
        {
            if (!IsBase64(eStr))
            {
                return eStr;
            }
            byte[] buffer = Convert.FromBase64String(eStr);
            string sourthUrl = Encoding.GetString(buffer);
            sourthUrl = HttpUtility.UrlDecode(sourthUrl);
            return sourthUrl;
        }
        /// <summary>
        /// 是否是Base64字符串
        /// </summary>
        /// <param name="eStr"></param>
        /// <returns></returns>
        public static bool IsBase64(string eStr)
        {
            if ((eStr.Length % 4) != 0)
            {
                return false;
            }
            if (!Regex.IsMatch(eStr, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 添加URL参数
        /// <summary>
        /// 添加URL参数
        /// 若参数存在,则更新,不区分key大小写
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public static string AddParam(string url, string paramName, string value)
        {
            string keyWord = paramName + "=";
            if (url.ToLower().IndexOf(keyWord.ToLower(), StringComparison.Ordinal) >= 0)
            {
                //参数存在,则更新
                return UpdateParam(url, paramName, value);
            }

            Uri uri = new Uri(url);
            if (string.IsNullOrEmpty(uri.Query))
            {
                string eval = HttpUtility.UrlEncode(value);
                eval = string.IsNullOrEmpty(eval) ? eval : eval.Replace("+", "%20");

                int index = url.IndexOf("#", StringComparison.Ordinal);
                if (index > 0)
                {
                    return url.Insert(index, "?" + paramName + "=" + eval);
                }
                return String.Concat(url, "?" + paramName + "=" + eval);
            }
            else
            {
                string eval = HttpUtility.UrlEncode(value);
                eval = string.IsNullOrEmpty(eval) ? eval : eval.Replace("+", "%20");

                int index = url.IndexOf("#", StringComparison.Ordinal);
                if (index > 0)
                {
                    return url.Insert(index, "&" + paramName + "=" + eval);
                }
                return String.Concat(url, "&" + paramName + "=" + eval);
            }
        }
        #endregion

        #region 更新URL参数
        /// <summary>
        /// 更新URL参数
        /// 若参数不存在,则添加,不区分key大小写
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public static string UpdateParam(string url, string paramName, string value)
        {
            string lowerUrl = url.ToLower();
            string keyWord = paramName + "=";
            if (lowerUrl.IndexOf(keyWord.ToLower(), StringComparison.Ordinal) < 0)
            {
                //参数不存在,则添加
                return AddParam(url, paramName, value);
            }
            int index = lowerUrl.IndexOf(keyWord.ToLower(), StringComparison.Ordinal);// + keyWord.Length;
            int index1 = url.IndexOf("&", index, StringComparison.Ordinal);
            if (index1 == -1)
            {
                url = url.Remove(index, url.Length - index);
                url = string.Concat(url, keyWord + value);
                return url;
            }
            url = url.Remove(index, index1 - index);
            url = url.Insert(index, keyWord + value);
            return url;
        }
        #endregion

        #region 删除URL参数
        /// <summary>
        /// 删除URL参数
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="paramName">参数名称</param>
        /// <returns></returns>
        public static string DeleteParam(string url, string paramName)
        {
            string lowerUrl = url.ToLower();
            string keyWord = paramName + "=";
            if (lowerUrl.IndexOf(keyWord.ToLower(), StringComparison.Ordinal) < 0)
            {
                //参数不存在,直接返回
                return url;
            }
            int index = lowerUrl.IndexOf(keyWord.ToLower(), StringComparison.Ordinal);// + keyWord.Length;
            int index1 = url.IndexOf("&", index, StringComparison.Ordinal);
            if (index1 == -1)
            {
                url = url.Remove(index - 1, url.Length - index + 1);
                return url;
            }
            url = url.Remove(index, index1 - index + 1);
            return url;
        }
        #endregion

        #region 分析URL所属的域
        public static void GetDomain(string fromUrl, out string domain, out string subDomain)
        {
            try
            {
                if (fromUrl.IndexOf("的名片", StringComparison.Ordinal) > -1)
                {
                    subDomain = fromUrl;
                    domain = "名片";
                    return;
                }
                UriBuilder builder = new UriBuilder(fromUrl);
                fromUrl = builder.ToString();
                Uri u = new Uri(fromUrl);
                if (u.IsWellFormedOriginalString())
                {
                    if (u.IsFile)
                    {
                        subDomain = domain = "客户端本地文件路径";
                    }
                    else
                    {
                        string authority = u.Authority;
                        string[] ss = u.Authority.Split('.');
                        if (ss.Length == 2)
                        {
                            authority = "www." + authority;
                        }
                        int index = authority.IndexOf('.', 0);
                        domain = authority.Substring(index + 1, authority.Length - index - 1).Replace("comhttp", "com");
                        subDomain = authority.Replace("comhttp", "com");
                        if (ss.Length < 2)
                        {
                            domain = "不明路径";
                            subDomain = "不明路径";
                        }
                    }
                }
                else
                {
                    if (u.IsFile)
                    {
                        subDomain = domain = "客户端本地文件路径";
                    }
                    else
                    {
                        subDomain = domain = "不明路径";
                    }
                }
            }
            catch
            {
                subDomain = domain = "不明路径";
            }
        }
        #endregion

        #region 分析URL字符串中的参数信息
        /// <summary>
        /// 分析URL字符串中的参数信息。
        /// </summary>
        /// <param name="url">输入的 URL</param>
        /// <param name="baseUrl">输出 URL 的基础部分</param>
        /// <param name="nvc">输出分析后得到的 (参数名,参数值) 的集合</param>
        public static void ParseUrl(string url, out string baseUrl, out NameValueCollection nvc)
        {
            if (url == null)
                throw new ArgumentNullException("url");
            nvc = new NameValueCollection();
            baseUrl = "";
            if (url == "")
                return;
            int questionMarkIndex = url.IndexOf('?');
            if (questionMarkIndex == -1)
            {
                baseUrl = url;
                return;
            }
            baseUrl = url.Substring(0, questionMarkIndex);
            if (questionMarkIndex == url.Length - 1)
                return;
            string ps = url.Substring(questionMarkIndex + 1);
            // 开始分析参数对  
            Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
            MatchCollection mc = re.Matches(ps);
            foreach (Match m in mc)
            {
                nvc.Add(m.Result("$2").ToLower(), m.Result("$3"));
            }
        }
        #endregion
    }
}