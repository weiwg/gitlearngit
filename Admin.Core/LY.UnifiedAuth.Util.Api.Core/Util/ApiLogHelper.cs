/* ******************************************************
 * 作者：weig
 * 功能：API日志记录类
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20191024 weigang  创建   
 ***************************************************** */

using System;
using System.IO;
using LY.UnifiedAuth.Util.Api.Core.Configs;

namespace LY.UnifiedAuth.Util.Api.Core.Util
{
    /// <summary>
    /// API日志记录类
    /// 版本：1.0
    /// <author>
    ///		<name>lzc</name>
    ///		<date>2019.10.24</date>
    /// </author>
    /// </summary>
    public class ApiLogHelper
    {
        #region 内部变量
        /// <summary>
        /// 只读对象用于锁
        /// </summary>
        private static readonly object WriteFile = new object();

        private static string _baseDirectPath;
        /// <summary>
        /// 基础路径
        /// </summary>
        private static string BaseDirectPath
        {
            get
            {
                _baseDirectPath = GetDirectPath();
                return _baseDirectPath;
            }
        }

        #endregion

        /// <summary>
        /// 写入DEBUG日志
        /// </summary>
        /// <param name="msg">输出信息</param>
        public static void Debug(string msg)
        {
            WriteLog(msg, "debug");
        }

        /// <summary>
        /// 写入普通日志
        /// </summary>
        /// <param name="msg">输出信息</param>
        public static void Info(string msg)
        {
            WriteLog(msg, "info");
        }

        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="msg">输出信息</param>
        public static void Error(string msg)
        {
            WriteLog(msg, "error");
        }

        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="ex">错误信息</param>
        /// <param name="msg">输出信息</param>
        public static void Error(Exception ex, string msg = "")
        {
            WriteLog(msg, "error", ex);
        }

        /// <summary>
        /// 在本地写入日志
        /// </summary>
        /// <param name="msg">日志描述</param>
        /// <param name="logType">日志类型</param>
        /// <param name="ex">错误信息</param>
        private static void WriteLog(string msg, string logType, Exception ex = null)
        {
            lock (WriteFile)
            {
                try
                {
                    var dtNow = DateTime.Now;
                    var directPath = BaseDirectPath + $@"\{dtNow.ToString("yyyy-MM-dd")}_{logType}.log";
                    using (var sw = GetStreamWriter(directPath))
                    {
                        //sw.WriteLine("***********************************************************************");
                        sw.WriteLine(dtNow.ToString("yyyy-MM-dd HH:mm:ss"));
                        //_streamWriter.WriteLine("信息类型：" + logType);
                        if (ex != null)
                        {
                            sw.WriteLine("异常：" + ex.Message);
                        }

                        sw.WriteLine(msg + "\r\n");

                        sw.Flush();
                        sw.Close();
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }

        #region 内部方法

        private static StreamWriter GetStreamWriter(string filPath)
        {
            return !File.Exists(filPath) ? File.CreateText(filPath) : File.AppendText(filPath);
        }

        private static string GetDirectPath()
        {
            var eupAuthConfig = ApiConfigHelper.Get<LYAuthConfig>("eupauthconfig") ?? new LYAuthConfig();
            var directPath = string.IsNullOrEmpty(eupAuthConfig.LogFilePath) ? "Log/ApiLog" : eupAuthConfig.LogFilePath + "/ApiLog";

            if (!string.IsNullOrEmpty(AppContext.BaseDirectory))
            {
                directPath = Path.Combine(AppContext.BaseDirectory, directPath);
            }
            else //非web程序引用  
            {
                directPath = directPath.Replace("/", "\\");
                directPath = directPath.Replace("\\\\", "\\");
                if (directPath.StartsWith("~")) //确定 String 实例的开头是否与指定的字符串匹配。为下边的合并字符串做准备
                {
                    directPath = directPath.TrimStart('~'); //从此实例的开始位置移除数组中指定的一组字符的所有匹配项。为下边的合并字符串做准备
                }

                if (directPath.StartsWith("\\"))
                {
                    directPath = directPath.TrimStart('\\');
                }

                //AppDomain表示应用程序域，它是一个应用程序在其中执行的独立环境　　　　　　　
                //AppDomain.CurrentDomain 获取当前 Thread 的当前应用程序域。
                //BaseDirectory 获取基目录，它由程序集冲突解决程序用来探测程序集。
                //AppDomain.CurrentDomain.BaseDirectory综合起来就是返回此代码所在的路径
                //System.IO.Path.Combine合并两个路径字符串
                //Path.Combine(@"C:\11","aa.txt") 返回的字符串路径如后： C:\11\aa.txt
                directPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directPath);
            }

            //记录错误日志文件的路径
            if (!Directory.Exists(directPath))
            {
                Directory.CreateDirectory(directPath);
            }

            return directPath;
        }

        #endregion
    }
}
