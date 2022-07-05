/* ******************************************************
 * 版权：广东易昂普软件信息有限公司
 * 作者：卢志成
 * 功能：输入格式验证帮助类工具
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20181120 luzhicheng  创建   
 ***************************************************** */

using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LY.Report.Core.Util.Verification
{
    /// <summary>
    /// 输入格式验证帮助工具
    /// </summary>
    public class VerifyHelper
    {
        private static readonly Regex RegNumber = new Regex("^[0-9]+$");
        private static readonly Regex RegPayPassword = new Regex(@"^\d{6}$");
        private static readonly Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private static readonly Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        private static readonly Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
        private static readonly Regex RegNumberAndDecimal = new Regex("^[0-9]+[.]?[0-9]+$|^[0-9]+$");
        private static readonly Regex RegNumberAndDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$|^[+-]?[0-9]+$");
        private static readonly Regex RegEmail = new Regex(@"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
        private static readonly Regex RegChzn = new Regex("[\u4e00-\u9fa5]");
        private static readonly Regex RegNumberWord = new Regex("^[A-Za-z0-9]+$");
        private static readonly Regex RegWord = new Regex("^[A-Za-z]+$");

        #region 用户名密码格式
        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns>字符长度</returns>
        public static int GetStringLength(string stringValue)
        {
            return Encoding.Default.GetBytes(stringValue).Length;
        }

        /// <summary>
        /// 检测用户名格式是否有效
        /// 判断用户名的长度（4-20个字符）及内容（只能是字母、下划线、数字）
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static bool IsValidUserName(string userName, int minLength = 4, int maxLength = 20)
        {
            int userNameLength = GetStringLength(userName);
            if (userNameLength >= minLength && userNameLength <= maxLength && Regex.IsMatch(userName, @"^([A-Za-z_0-9]{4,20})$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检测密码格式是否有效
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsValidPassword(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            return Regex.IsMatch(inputStr, @"^[A-Za-z_0-9]{6,20}$");
        }

        /// <summary>
        /// 初级密码（校验密码6-16字符，必须包含字母、数字）
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsLowPassword(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            return Regex.IsMatch(inputStr, @"^(?=.*[0-9])(?=.*[a-zA-Z]).{6,16}$");
        }

        /// <summary>
        /// 中级密码（校验密码6-16字符 必须包含字母、数字、特称字符）
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsMiddlePassword(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            return Regex.IsMatch(inputStr, @"^(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[^a-zA-Z0-9]).{6,16}$");
        }

        /// <summary>
        /// 高级密码（校验密码6-16字符 必须包含大小字母、数字、特称字符）
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsHighPassword(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            return Regex.IsMatch(inputStr, @"^(?=.*[0-9])(?=.*[A-Z])(?=.*[a-z])(?=.*[^a-zA-Z0-9]).{6,16}$");
        }

        /// <summary>
        /// 验证支付密码（六位数字且不完全相同的支付密码）
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <returns></returns>
        public static bool IsPayPassword(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            Match m = RegPayPassword.Match(inputStr);
            bool result = m.Success;
            if (result)
            {
                string[] numbers = { "000000", "111111", "222222", "333333", "444444", "555555", "666666", "777777", "888888", "999999" };
                result = !numbers.Contains(inputStr);
            }
            return result;
        }
        #endregion

        #region 数字字符串检查
        /// <summary>
        /// int有效性
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsValidInt(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            return Regex.IsMatch(inputStr, @"^[1-9]\d*\.?[0]*$");
        }

        /// <summary>
        /// 是否数字字符串
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            Match m = RegNumber.Match(inputStr);
            return m.Success;
        }

        /// <summary>
        /// 是否数字字符串 可带正负号
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumberSign(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            Match m = RegNumberSign.Match(inputStr);
            return m.Success;
        }

        /// <summary>
        /// 是否是浮点数
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            Match m = RegDecimal.Match(inputStr);
            return m.Success;
        }

        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimalSign(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            Match m = RegDecimalSign.Match(inputStr);
            return m.Success;
        }

        /// <summary>
        /// 是否是浮点数或整数
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumberAndDecimal(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            Match m = RegNumberAndDecimal.Match(inputStr);
            return m.Success;
        }

        /// <summary>
        /// 是否是浮点数或整数 可带正负号
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumberAndDecimalSign(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            Match m = RegNumberAndDecimalSign.Match(inputStr);
            return m.Success;
        }
        #endregion

        #region 字母和数字
        /// <summary>
        /// 判断是存在全角字符
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsFullAngle(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            Match m = new Regex("[^\x00-\xff]").Match(inputStr);
            return m.Success;
        }

        public static bool IsNumWord(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            Match m = RegNumberWord.Match(inputStr);
            return m.Success;
        }
        public static bool IsWord(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            Match m = RegWord.Match(inputStr);
            return m.Success;
        }
        #endregion

        #region SQL检查
        /// <summary>
        /// 验证是否有SQL注入式攻击代码
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool ValidSql(string val)
        {
            return ProcessSqlStr(val);
        }

        /// <summary> 
        /// 分析用户请求是否正常 
        /// </summary> 
        /// <param name="str">传入用户提交数据</param> 
        /// <returns>返回是否含有SQL注入式攻击代码</returns> 
        public static bool ProcessSqlStr(string str)
        {
            if (str.Trim() != "")
            {
                string sqlStr = "# |$ |% |& |* |' |< |> |% |AND |SELECT |INSERT |DELETE |UPDATE |EXEC |COUNT |* |CHAR |CHR |MID |MASTER |TRUNCATE |DECLARE";
                //string SqlStr = "exec |insert |select |delete |update |mid |master |truncate |declare";
                string[] anySqlStr = sqlStr.Split('|');
                foreach (string ss in anySqlStr)
                {
                    if (str.ToLower().IndexOf(ss.Trim(), StringComparison.Ordinal) >= 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region 中文检测
        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsHasChzn(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            Match m = RegChzn.Match(inputStr);
            return m.Success;
        }

        /// <summary> 
        /// 检测含有中文字符串的实际长度 
        /// </summary> 
        /// <param name="inputStr">字符串</param> 
        public static int GetChznLength(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return 0;
            }
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            byte[] bytes = asciiEncoding.GetBytes(inputStr);

            int length = 0; // l 为字符串之实际长度 
            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                if (bytes[i] == 63) //判断是否为汉字或全脚符号 
                {
                    length++;
                }
                length++;
            }
            return length;
        }

        #endregion

        #region 常用格式
        /// <summary>
        /// 验证身份证是否合法  15 和  18位两种
        /// </summary>
        /// <param name="inputStr">要验证的身份证</param>
        public static bool IsIdCard(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            if (inputStr.Length == 15)
            {
                return Regex.IsMatch(inputStr, @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$");
            }
            if (inputStr.Length == 18)
            {
                return Regex.IsMatch(inputStr, @"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$", RegexOptions.IgnoreCase);
            }
            return false;
        }

        /// <summary>
        /// 是否是邮件地址
        /// </summary>
        /// <param name="inputStr">输入字符串</param>
        /// <returns></returns>
        public static bool IsEmail(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            Match m = RegEmail.Match(inputStr);
            return m.Success;
        }

        /// <summary>
        /// 邮编有效性
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsValidZip(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            Regex rx = new Regex(@"^\d{6}$", RegexOptions.None);
            Match m = rx.Match(inputStr);
            return m.Success;
        }

        /// <summary>
        /// 固定电话有效性
        /// 010 -1234567 , 0101234567 , (010)1234567
        /// 010 -12345678, 01012345678, (010)12345678
        /// 0110-12345678,011012345678,(0110)12345678
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsValidFixedPhone(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            Regex rx = new Regex(@"^((\([0]\d{2,3}\))|[0](\d{2,3}|\d{2,3}-))?\d{7,8}$", RegexOptions.None);
            Match m = rx.Match(inputStr);
            return m.Success;
        }

        /// <summary>
        /// 手机有效性
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsValidMobile(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            Regex rx = new Regex(@"^[1](3|4|5|7|8)\d{9}$|^[1](66|91|93|98|99)\d{8}$", RegexOptions.None);
            Match m = rx.Match(inputStr);
            return m.Success;
        }

        /// <summary>
        /// 电话有效性（固话和手机 ）
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsValidPhoneAndMobile(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            Regex rx = new Regex(@"^((\([0]\d{2,3}\))|[0](\d{2,3}|\d{2,3}-))?\d{7,8}$|^[1](3|4|5|7|8)\d{9}$|^[1](66|98|99)\d{8}$", RegexOptions.None);
            Match m = rx.Match(inputStr);
            return m.Success;
        }

        /// <summary>
        /// Url有效性
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsValidUrl(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            return Regex.IsMatch(inputStr, @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&%\$#\=~])*[^\.\,\)\(\s]$");
        }

        /// <summary>
        /// Url有效性
        /// 匹配 http://site.com/dir/file.php?var=moo | https://localhost |ftp://user:pass@site.com:21/file/dir
        /// 不匹配 site.com | http://site.com/dir//
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsValidUrl2(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            return Regex.IsMatch(inputStr, @"^(http|https|ftp)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&amp;%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&amp;%\$#\=~_\-]+))*$");
        }

        /// <summary>
        /// IP有效性
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsValidIp(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            return Regex.IsMatch(inputStr, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// domain 有效性
        /// </summary>
        /// <param name="inputStr">域名</param>
        /// <returns></returns>
        public static bool IsValidDomain(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            Regex reg = new Regex(@"^\d+$");
            if (inputStr.IndexOf(".", StringComparison.Ordinal) == -1)
            {
                return false;
            }
            return !reg.IsMatch(inputStr.Replace(".", string.Empty));
        }

        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsBase64String(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            return Regex.IsMatch(inputStr, @"[A-Za-z0-9\+\/\=]");
        }

        /// <summary>
        /// 验证字符串是否是GUID
        /// </summary>
        /// <param name="inputStr">字符串</param>
        /// <returns></returns>
        public static bool IsGuid(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            return Regex.IsMatch(inputStr, "[A-F0-9]{8}(-[A-F0-9]{4}){3}-[A-F0-9]{12}|[A-F0-9]{32}", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 日期检查
        /// <summary>
        /// 判断输入的字符是否为日期
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsDate(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            return Regex.IsMatch(inputStr, @"^((\d{2}(([02468][048])|([13579][26]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9])))))|(\d{2}(([02468][1235679])|([13579][01345789]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))))");
        }

        /// <summary>
        /// 判断输入的字符是否为日期,如2004-07-12 14:25|||1900-01-01 00:00|||9999-12-31 23:59
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static bool IsDateHourMinute(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }
            return Regex.IsMatch(inputStr, @"^(19[0-9]{2}|[2-9][0-9]{3})-((0(1|3|5|7|8)|10|12)-(0[1-9]|1[0-9]|2[0-9]|3[0-1])|(0(4|6|9)|11)-(0[1-9]|1[0-9]|2[0-9]|30)|(02)-(0[1-9]|1[0-9]|2[0-9]))\x20(0[0-9]|1[0-9]|2[0-3])(:[0-5][0-9]){1}$");
        }
        #endregion

        #region 其他
        /// <summary>
        /// 检查字符串最大长度，返回指定长度的串
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>
        public static string CheckMathLength(string inputData, int maxLength)
        {
            if (!string.IsNullOrEmpty(inputData))
            {
                inputData = inputData.Trim();
                if (inputData.Length > maxLength)//按最大长度截取字符串
                {
                    inputData = inputData.Substring(0, maxLength);
                }
            }
            return inputData;
        }

        /// <summary>
        /// 转换成 HTML code
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string Encode(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }
        /// <summary>
        ///解析html成 普通文本
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string Decode(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }
        #endregion
    }
}
