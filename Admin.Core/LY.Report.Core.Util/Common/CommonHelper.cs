/* ******************************************************
 * 作者：weig
 * 功能：公共帮助类
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20181120 weigang  创建   
 ***************************************************** */

using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using LY.Report.Core.Util.Verification;

namespace LY.Report.Core.Util.Common
{
    /// <summary>
    /// 公共帮助类
    /// </summary>
    public class CommonHelper
    {
        #region 类型转换
        /// <summary>
        /// 返回对象obj的String值,obj为null时返回空值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>字符串。</returns>
        public static string GetObjectString(object obj)
        {
            return null == obj ? String.Empty : obj.ToString();
        }

        /// <summary>
        /// 取得int值，如果为Null 则返回 0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetInt(object obj)
        {
            if (obj != null && obj.ToString() != "")
            {
                int i;
                int.TryParse(obj.ToString(), out i);
                return i;
            }
            return 0;
        }

        /// <summary>
        /// 取得float值，如果为Null 则返回 0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static float GetFloat(object obj)
        {
            float i;
            float.TryParse(obj.ToString(), out i);
            return i;
        }

        /// <summary>
        /// 取得double值，如果为Null 则返回 0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double GetDouble(object obj)
        {
            double i;
            double.TryParse(Convert.ToString(obj), out i);
            return i;
        }

        /// <summary>
        /// 取得byte值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte Getbyte(object obj)
        {
            if (obj != null && obj.ToString() != "")
            {
                byte i;
                byte.TryParse(obj.ToString(), out i);
                return i;
            }
            return 0;
        }

        /// <summary>
        /// 获得Long值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long GetLong(object obj)
        {
            if (obj != null && obj.ToString() != "")
            {
                long i;
                long.TryParse(obj.ToString(), out i);
                return i;
            }
            return 0;
        }

        /// <summary>
        /// 取得Decimal值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal GetDecimal(object obj)
        {
            if (obj != null && obj.ToString() != "")
            {
                decimal i;
                decimal.TryParse(obj.ToString(), out i);
                return i;
            }
            return 0;
        }

        /// <summary>
        /// 取得DateTime值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(object obj)
        {
            if (obj != null && obj.ToString() != "")
            {
                DateTime dateTime;
                DateTime.TryParse(obj.ToString(), out dateTime);
                return dateTime;
            }
            return DateTime.Now.ToLocalTime();
        }

        /// <summary>
        /// 取得DateTime值
        /// value  : 20060709110901
        /// format : yyyyMMddHHmmss
        /// </summary>
        /// <param name="value">时间值</param>
        /// <param name="format">时间值的格式</param>
        /// <returns></returns>
        public static DateTime GetDateTimeByFormat(string value, string format)
        {
            if (!string.IsNullOrEmpty(value))
            {
                DateTime dateTime;
                DateTime.TryParseExact(value, format, CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTime);
                return dateTime;
            }
            return DateTime.Now.ToLocalTime();
        }

        /// <summary>
        /// 取得DateTime值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format">yyyy-MM-dd HH:mm:ss</param>
        /// <returns></returns>
        public static string GetDateTime(object obj, string format)
        {
            return GetDateTime(obj).ToString(format);
        }

        /// <summary>
        /// 取得DateTime值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(object obj)
        {
            if (obj != null && obj.ToString() != "")
            {
                DateTime dateTime;
                DateTime.TryParse(obj.ToString(), out dateTime);
                return dateTime;
            }
            return DateTime.Now.ToLocalTime();
        }

        /// <summary>
        /// 取得bool值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetBool(object obj)
        {
            if (obj != null)
            {
                bool flag;
                bool.TryParse(obj.ToString(), out flag);
                return flag;
            }
            return false;
        }

        /// <summary>
        /// 取得byte[]
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Byte[] GetByte(object obj)
        {
            if (obj.ToString() != "")
            {
                return (Byte[])obj;
            }
            return null;
        }

        /// <summary>
        /// 取得string值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetString(object obj)
        {
            if (obj != null && obj != DBNull.Value)
            {
                return obj.ToString().Trim();
            }
            return "";
        }

        /// <summary>   
        /// 判断用户输入是否为日期   
        /// </summary>   
        /// <param ></param>
        /// <param name="strValue"></param>
        /// <returns></returns>   
        /// <remarks>   
        /// 可判断格式如下（其中-可替换为.，不影响验证)   
        /// YYYY | YYYY-MM |YYYY.MM| YYYY-MM-DD|YYYY.MM.DD | YYYY-MM-DD HH:MM:SS | YYYY.MM.DD HH:MM:SS | YYYY-MM-DD HH:MM:SS.FFF | YYYY.MM.DD HH:MM:SS:FF (年份验证从1000到2999年)
        /// </remarks>   
        public static bool IsDateTime(string strValue)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                return false;
            }
            string regexDate = @"[1-2]{1}[0-9]{3}((-|[.]){1}(([0]?[1-9]{1})|(1[0-2]{1}))((-|[.]){1}((([0]?[1-9]{1})|([1-2]{1}[0-9]{1})|(3[0-1]{1})))( (([0-1]{1}[0-9]{1})|2[0-3]{1}):([0-5]{1}[0-9]{1}):([0-5]{1}[0-9]{1})(\.[0-9]{3})?)?)?)?$";
            if (Regex.IsMatch(strValue, regexDate))
            {
                //以下各月份日期验证，保证验证的完整性   
                int indexY;
                int indexM;
                int indexD;
                if (-1 != (indexY = strValue.IndexOf("-", StringComparison.Ordinal)))
                {
                    indexM = strValue.IndexOf("-", indexY + 1, StringComparison.Ordinal);
                    indexD = strValue.IndexOf(":", StringComparison.Ordinal);
                }
                else
                {
                    indexY = strValue.IndexOf(".", StringComparison.Ordinal);
                    indexM = strValue.IndexOf(".", indexY + 1, StringComparison.Ordinal);
                    indexD = strValue.IndexOf(":", StringComparison.Ordinal);
                }
                //不包含日期部分，直接返回true   
                if (-1 == indexM)
                {
                    return true;
                }
                if (-1 == indexD)
                {
                    indexD = strValue.Length + 3;
                }
                int iYear = Convert.ToInt32(strValue.Substring(0, indexY));
                int iMonth = Convert.ToInt32(strValue.Substring(indexY + 1, indexM - indexY - 1));
                int iDate = Convert.ToInt32(strValue.Substring(indexM + 1, indexD - indexM - 4));
                //判断月份日期   
                if ((iMonth < 8 && 1 == iMonth % 2) || (iMonth > 8 && 0 == iMonth % 2))
                {
                    if (iDate < 32)
                    { return true; }
                }
                else
                {
                    if (iMonth != 2)
                    {
                        if (iDate < 31)
                        { return true; }
                    }
                    else
                    {
                        //闰年   
                        if ((0 == iYear % 400) || (0 == iYear % 4 && 0 < iYear % 100))
                        {
                            if (iDate < 30)
                            { return true; }
                        }
                        else
                        {
                            if (iDate < 29)
                            { return true; }
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        #region "全球唯一码GUID"
        /// <summary>
        /// 获取一个全球唯一码GUID字符串
        /// 57D99D89CAAB482AA0E9A0A803EED3BA
        /// </summary>
        public static string GetGuid => Guid.NewGuid().ToString("N").ToUpper();

        /// <summary>
        /// 获取一个全球唯一码GUID字符串
        /// 57D99D89-CAAB-482A-A0E9-A0A803EED3BA
        /// </summary>
        public static string GetGuidD => Guid.NewGuid().ToString("D").ToUpper();

        /// <summary>
        /// 获取一个全球唯一码GUID字符串
        /// </summary>
        /// <param name="format">格式</param>
        /// <returns></returns>
        public static string GetGuidFormat(string format)
        {
            return Guid.NewGuid().ToString(format).ToUpper();
        }
        #endregion

        #region 获取unix时间戳
        /// <summary>
        ///  时间戳转本地时间-时间戳精确到秒
        /// </summary> 
        public static DateTime ToLocalTimeDateBySeconds(long unix)
        {
            var dto = DateTimeOffset.FromUnixTimeSeconds(unix);
            return dto.ToLocalTime().DateTime;
        }

        /// <summary>
        ///  时间戳转本地时间-时间戳精确到毫秒
        /// </summary> 
        public static DateTime ToLocalTimeDateByMilliseconds(long unix)
        {
            var dto = DateTimeOffset.FromUnixTimeMilliseconds(unix);
            return dto.ToLocalTime().DateTime;
        }

        /// <summary>
        ///  时间转时间戳Unix-时间戳精确到秒
        /// </summary> 
        public static long ToTimestampBySeconds(DateTime dt)
        {
            DateTimeOffset dto = new DateTimeOffset(dt);
            return dto.ToUnixTimeSeconds();
        }

        /// <summary>
        ///  时间转时间戳Unix-时间戳精确到毫秒
        /// </summary> 
        public static long ToTimestampByMilliseconds(DateTime dt)
        {
            DateTimeOffset dto = new DateTimeOffset(dt);
            return dto.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 获取unix时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的总秒数
        /// </summary>
        /// <returns></returns>
        public static string GetTimestamp()
        {
            //return ToTimestampBySeconds(DateTime.UtcNow).ToString();
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        #endregion

        #region 生成0-9随机数
        /// <summary>
        /// 生成0-9随机数
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="isUserSeed">是否加入种子随机</param>
        /// <returns></returns>
        public static string GetRandomNum(int length, bool isUserSeed = true)
        {
            string randomNum = "";
            for (int i = 0; i < length; i++)
            {
                Random rand = isUserSeed
                    ? new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0))
                    : new Random();
                randomNum += rand.Next(10);
            }
            return randomNum;
        }
        #endregion

        #region 生成随机字符
        /// <summary>
        /// 生成随机字符
        /// </summary>
        /// <param name="codeNum">生成长度</param>
        /// <returns></returns>
        public static string RandomCodeStr(int codeNum)
        {
            char[] pattern = new char[]
            {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
                'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
            };
            string codeStr = "";
            int n = pattern.Length;
            Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < codeNum; i++)
            {
                int rnd = random.Next(0, n);
                codeStr += pattern[rnd];
            }
            return codeStr;
        }
        #endregion

        #region 路径转换（转换成绝对路径）
        /// <summary>
        /// 路径转换（转换成绝对路径）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string WebPathTran(string path)
        {
            //try
            //{
            //    return HttpContext.Current.Server.MapPath(path);
            //}
            //catch
            //{
            //    return path;
            //}
            return "";
        }
        #endregion

        #region 获取用户操作平台
        /// <summary>
        /// 获取用户操作平台
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
            if (mobileOs.Any(s => userAgent.IndexOf(s, StringComparison.Ordinal) > -1))
            {
                //微信
                if (userAgent.Contains("micromessenger"))
                {
                    return 1011;
                }
                return 1010;
            }
            //PC端
            if (userAgent.Contains("windows nt") || userAgent.Contains("macintosh"))
            {
                return 1000;
            }
            //其他设备(禁止访问)
            string[] forbidOs = { "jodd http", "mifetcher/1.0", "baiduspider" };
            if (forbidOs.Any(s => userAgent.IndexOf(s, StringComparison.Ordinal) > -1))
            {
                return -1;
            }
            //其他未知
            return 0;
        }

        /// <summary>
        /// 判断是否微信浏览器
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public static bool IsWeChatPlatform(string userAgent)
        {
            if (!string.IsNullOrEmpty(userAgent) && userAgent.ToLower().Contains("micromessenger"))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 字符串加密
        /// <summary>
        /// 字符串加密
        /// 例:CommonHelper.StringEncrypt(test, 3, 4);
        /// 原文>>132123456789
        /// 返回>>132*****6789
        /// </summary>
        /// <param name="text">待加密字符串</param>
        /// <param name="begin">开始位数</param>
        /// <param name="end">结束位数</param>
        /// <param name="encryptCount">指定加密字符长度,0不指定</param>
        /// <returns></returns>
        public static string StringEncrypt(string text, int begin, int end, int encryptCount = 0)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            if (text.Length < (begin + end + 1))
            {
                return text;
            }

            string encryptStr = encryptCount <= 0 ? "*".PadLeft(text.Length - begin - end, '*') : "*".PadLeft(encryptCount, '*');
            return text.Substring(0, begin) + encryptStr + text.Substring(text.Length - end, end);
        }

        /// <summary>
        /// 字符串加密
        /// 例:CommonHelper.StringEncrypt(test, 3, 4);
        /// 原文>>132123456789
        /// 返回>>132*****6789
        /// </summary>
        /// <param name="text">待加密字符串</param>
        /// <returns></returns>
        public static string StringEncryptEmail(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            return StringEncrypt(text, 3, text.LastIndexOf('@') < 0 ? 0 : text.Length - text.LastIndexOf('@'), 4);
        }

        /// <summary>
        /// 字符串加密
        /// 例:CommonHelper.StringEncrypt(test, 3, 4);
        /// 原文>>132123456789
        /// 返回>>132*****6789
        /// </summary>
        /// <param name="text">待加密字符串</param>
        /// <returns></returns>
        public static string StringEncryptPhone(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            return StringEncrypt(text, 3, 4);
        }

        /// <summary>
        /// 字符串加密
        /// 例:CommonHelper.StringEncrypt(test, 3, 4);
        /// 原文>>132123456789
        /// 返回>>132*****6789
        /// </summary>
        /// <param name="text">待加密字符串</param>
        /// <returns></returns>
        public static string StringEncryptIdCard(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return "";
            }
            return StringEncrypt(text, 4, 3);
        }

        /// <summary>
        /// 字符串加密(自动匹配,支持手机,邮箱)
        /// 例:CommonHelper.StringEncrypt(test, 3, 4);
        /// 原文>>132123456789
        /// 返回>>132*****6789
        /// </summary>
        /// <param name="text">待加密字符串</param>
        /// <param name="stringEncryptType">加密类型</param>
        /// <returns></returns>
        public static string StringEncryptAuto(string text, StringEncryptType stringEncryptType = StringEncryptType.Auto)
        {
            if (stringEncryptType == StringEncryptType.Auto)
            {
                if (VerifyHelper.IsEmail(text))
                {
                    return StringEncryptEmail(text);
                }
                if (VerifyHelper.IsValidMobile(text))
                {
                    return StringEncryptPhone(text);
                }
            }
            else if (stringEncryptType == StringEncryptType.Email)
            {
                return StringEncryptEmail(text);
            }
            else if (stringEncryptType == StringEncryptType.Phone)
            {
                return StringEncryptPhone(text);
            }
            else if (stringEncryptType == StringEncryptType.IdCard)
            {
                return StringEncryptIdCard(text);
            }
            else if (stringEncryptType == StringEncryptType.BandCard)
            {
                return StringEncrypt(text, 4, 4);
            }
            return StringEncrypt(text, 1, 1);
        }

        public enum StringEncryptType
        {
            Auto = 0,
            Phone = 1,
            Email = 2,
            IdCard = 3,
            BandCard = 4
        }

        #endregion
    }
}
