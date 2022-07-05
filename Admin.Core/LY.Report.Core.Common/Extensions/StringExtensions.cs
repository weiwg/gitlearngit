using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LY.Report.Core
{
    public static class StringExtensions
    {
        /// <summary>
        /// 判断字符串是否为Null、空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNull(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 判断字符串是否不为Null、空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotNull(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 与字符串进行比较，忽略大小写
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string str, string value)
        {
            return str.Equals(value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 首字母转小写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FirstCharToLower(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return str.First().ToString().ToLower() + str.Substring(1);
        }

        /// <summary>
        /// 首字母转大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            return str.First().ToString().ToUpper() + str.Substring(1);
        }

        /// <summary>
        /// 转为Base64，UTF-8格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToBase64(this string str)
        {
            return str.ToBase64(Encoding.UTF8);
        }

        /// <summary>
        /// 转为Base64
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string ToBase64(this string str, Encoding encoding)
        {
            if (str.IsNull())
            {
                return string.Empty;
            }

            var bytes = encoding.GetBytes(str);
            return bytes.ToBase64();
        }

        public static string ToPath(this string str)
        {
            if (str.IsNull())
            {
                return string.Empty;
            }

            return str.Replace(@"\", "/");
        }

        public static string Format(this string str, object obj)
        {
            if (str.IsNull())
            {
                return str;
            }
            string s = str;
            if (obj.GetType().Name == "JObject")
            {
                foreach (var item in (Newtonsoft.Json.Linq.JObject)obj)
                {
                    var k = item.Key.ToString();
                    var v = item.Value.ToString();
                    s = Regex.Replace(s, "\\{" + k + "\\}", v, RegexOptions.IgnoreCase);
                }
            }
            else
            {
                foreach (System.Reflection.PropertyInfo p in obj.GetType().GetProperties())
                {
                    var xx = p.Name;
                    var yy = p.GetValue(obj).ToString();
                    s = Regex.Replace(s, "\\{" + xx + "\\}", yy, RegexOptions.IgnoreCase);
                }
            }
            return s;
        }
    }
}
