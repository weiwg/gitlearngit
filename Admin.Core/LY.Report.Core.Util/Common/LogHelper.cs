/* ******************************************************
 * 版权：广东易昂普软件信息有限公司
 * 作者：卢志成
 * 功能：日志帮助类
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20190111 luzhicheng  创建   
 ***************************************************** */
 
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace LY.Report.Core.Util.Common
{
    /// <summary>
    ///  日志帮助类
    /// </summary>
    public class LogHelper : ILogHelper, IDisposable
    {
        #region 私有变量
        private static LogCongig _logCongig;
        private static LogCongig LogCongig => _logCongig ?? (_logCongig = ConfigHelper.Get<LogCongig>("logconfig") ?? new LogCongig());

        /// <summary>
        /// 对象用于锁
        /// </summary>
        private readonly Object _writeLogLockObj = new Object();
        /// <summary>
        /// 日志名称
        /// </summary>
        private readonly string _logFileName;

        /// <summary>
        /// 普通日志文件物理路径
        /// </summary>
        private readonly string _logFileNormalMapPath = LogCongig.LogFilePath;
        /// <summary>
        /// 调试日志文件物理路径
        /// </summary>
        private readonly string _logFileDebugMapPath = LogCongig.LogFilePath;
        /// <summary>
        /// 错误日志文件物理路径
        /// </summary>
        private readonly string _logFileMapErrorPath = LogCongig.LogFilePath;

        /// <summary>
        /// 获取是否写入日志
        /// </summary>
        private readonly bool _logIsWrite = CommonHelper.GetBool(LogCongig.LogIsWrite);

        /// <summary>
        /// 获取是否写入普通日志
        /// </summary>
        private readonly bool _isWirteNormalLog = CommonHelper.GetBool(LogCongig.LogIsWriteByNormal);

        /// <summary>
        /// 获取是否写入Debug日志
        /// </summary>
        private readonly bool _isWirteDebugLog = CommonHelper.GetBool(LogCongig.LogIsWriteByDebug);

        /// <summary>
        /// 获取是否写入Erro日志
        /// </summary>
        private readonly bool _isWirteErrorLog = CommonHelper.GetBool(LogCongig.LogIsWriteByError);

        /// <summary>
        /// 日志类型
        /// </summary>
        private enum LogType
        {
            Debug,
            Normal,
            Error
        }

        #region IDisposable 成员

        public void Dispose()
        {
        }

        #endregion

        #endregion

        #region 实例化
        /// <summary>
        /// 实例日志管理，以当前日期为文件名，如果文件不存在，则创建文件
        /// </summary>
        public LogHelper()
        {
            _logFileName = "";
        }

        /// <summary>
        /// 实例日志管理，如果文件不存在，则创建文件
        /// </summary>
        /// <param name="logFileName">创建txt文件名称</param>
        public LogHelper(string logFileName)
        {
            _logFileName = logFileName;
        }
        
        public static LogHelper Init
        {
            get
            {
                MethodBase mb = new StackTrace(true).GetFrame(1).GetMethod();
                string logName = mb.ReflectedType != null ? mb.ReflectedType.FullName : "";
                return new LogHelper(logName);
            }
        }
        #endregion

        #region 创建日志文件
        private void CreateLoggerFile(string fileName, string logPath, out string newLogPath)
        {
            newLogPath = "";
            if (_logIsWrite)//是否写日志
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = DateTime.Now.ToString("yyyy-MM-dd");
                }

                if (!string.IsNullOrEmpty(AppContext.BaseDirectory))
                {
                    logPath = Path.Combine(AppContext.BaseDirectory, logPath);
                }
                else //非web程序引用  
                {
                    logPath = logPath.Replace("/", "\\");
                    logPath = logPath.Replace("\\\\", "\\");
                    if (logPath.StartsWith("~"))//确定 String 实例的开头是否与指定的字符串匹配。为下边的合并字符串做准备
                    {
                        logPath = logPath.TrimStart('~');//从此实例的开始位置移除数组中指定的一组字符的所有匹配项。为下边的合并字符串做准备
                    }
                    if (logPath.StartsWith("\\"))
                    {
                        logPath = logPath.TrimStart('\\');
                    }
                    //AppDomain表示应用程序域，它是一个应用程序在其中执行的独立环境　　　　　　　
                    //AppDomain.CurrentDomain 获取当前 Thread 的当前应用程序域。
                    //BaseDirectory 获取基目录，它由程序集冲突解决程序用来探测程序集。
                    //AppDomain.CurrentDomain.BaseDirectory综合起来就是返回此代码所在的路径
                    //System.IO.Path.Combine合并两个路径字符串
                    //Path.Combine(@"C:\11","aa.txt") 返回的字符串路径如后： C:\11\aa.txt
                    logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logPath);
                }
                
                if (logPath.Length < 1)
                {
                    Console.WriteLine("配置文件中没有指定日志文件目录！");
                    return;
                }
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                if (!Directory.Exists(logPath))
                {
                    Console.WriteLine("配置文件中指定日志文件目录不存在！");
                    return;
                }
                if ((logPath.Substring((logPath.Length - 1), 1).Equals("/")) || (logPath.Substring(logPath.Length - 1, 1).Equals("\\")))
                {
                    newLogPath = logPath + fileName + ".log";
                }
                else
                {
                    newLogPath = logPath + "\\" + fileName + ".log";
                }
                try
                {
                    FileStream fs = new FileStream(newLogPath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                    fs.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
        #endregion

        #region 打开对应的日志文件
        /// <summary>
        /// 打开对应的日志文件
        /// </summary>
        private StreamWriter FileOpen(LogType logType)
        {
            string logFilePath;
            switch (logType)
            {
                case LogType.Debug:
                    logFilePath = string.Format("{0}//Debug//{1}", _logFileDebugMapPath, DateTime.Now.ToString("yyyy-MM-dd"));
                    break;
                case LogType.Normal:
                    logFilePath = string.Format("{0}//Normal//{1}", _logFileNormalMapPath, DateTime.Now.ToString("yyyy-MM-dd"));
                    break;
                case LogType.Error:
                    logFilePath = string.Format("{0}//Error//{1}", _logFileMapErrorPath, DateTime.Now.ToString("yyyy-MM-dd"));
                    break;
                default:
                    logFilePath = string.Format("{0}//Norma//{1}l", _logFileNormalMapPath, DateTime.Now.ToString("yyyy-MM-dd"));
                    break;
            }
            string newLogPath;
            CreateLoggerFile(_logFileName, logFilePath, out newLogPath);

            return new StreamWriter(newLogPath, true);
        }
        #endregion

        #region 写入日志内容

        #region 写入DEBUG日志
        /// <summary>
        /// 写入DEBUG日志
        /// </summary>
        /// <param name="msg">日志消息</param>
        public void Debug(string msg)
        {
            if (msg != null && _isWirteDebugLog)
            {
                WriteLog(msg, "调试信息", LogType.Debug);
            }
        }

        /// <summary>
        /// 写入DEBUG日志
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="msg">日志消息</param>
        public void Debug(string className, string msg)
        {
            if (msg != null && _isWirteDebugLog)
            {
                WriteLog(msg, "调试信息:" + className, LogType.Debug);
            }
        }

        /// <summary>
        /// 写入DEBUG日志
        /// </summary>
        /// <param name="msg">日志消息</param>
        /// <param name="isWriteClassInfo">是否写入调用类信息</param>
        /// <param name="frameIndex">获取指定的堆栈帧</param>
        public void Debug(string msg, bool isWriteClassInfo, int frameIndex = 1)
        {
            if (msg != null && _isWirteDebugLog)
            {
                WriteLog(msg, "调试信息", LogType.Debug, isWriteClassInfo, frameIndex + 1);
            }
        }
        #endregion

        #region 写入普通日志
        /// <summary>
        /// 写入普通日志
        /// </summary>
        /// <param name="msg">日志消息</param>
        public void Info(string msg)
        {
            if (msg != null && _isWirteNormalLog)
            {
                WriteLog(msg, "普通信息", LogType.Normal);
            }
        }

        /// <summary>
        /// 写入普通日志
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="msg">日志消息</param>
        public void Info(string className, string msg)
        {
            if (msg != null && _isWirteErrorLog)
            {
                WriteLog(msg, "普通信息:" + className, LogType.Normal);
            }
        }

        /// <summary>
        /// 写入普通日志
        /// </summary>
        /// <param name="msg">日志消息</param>
        /// <param name="isWriteClassInfo">是否写入调用类信息</param>
        /// <param name="frameIndex">获取指定的堆栈帧</param>
        public void Info(string msg, bool isWriteClassInfo, int frameIndex = 1)
        {
            if (msg != null && _isWirteDebugLog)
            {
                WriteLog(msg, "普通信息", LogType.Normal, isWriteClassInfo, frameIndex + 1);
            }
        }
        #endregion

        #region 写入错误日志
        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="msg">日志消息</param>
        public void Error(string msg)
        {
            if (msg != null && _isWirteErrorLog)
            {
                WriteLog(msg, "错误信息", LogType.Error);
            }
        }

        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="msg">日志消息</param>
        public void Error(string className, string msg)
        {
            if (msg != null && _isWirteErrorLog)
            {
                WriteLog(msg, "错误信息:" + className, LogType.Error);
            }
        }

        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="msg">日志消息</param>
        /// <param name="isWriteClassInfo">是否写入调用类信息</param>
        /// <param name="frameIndex">获取指定的堆栈帧</param>
        public void Error(string msg, bool isWriteClassInfo, int frameIndex = 1)
        {
            if (msg != null && _isWirteDebugLog)
            {
                WriteLog(msg, "错误信息", LogType.Error, isWriteClassInfo, frameIndex + 1);
            }
        }
        #endregion

        #region 向日志文件中写入日志Main
        /// <summary>
        /// 向日志文件中写入日志
        /// </summary>
        /// <param name="messagestr"></param>
        /// <param name="infoDescribe">日志描述</param>
        /// <param name="logType">日志类型</param>
        /// <param name="isWriteClassInfo">是否写入调用类信息</param>
        /// <param name="frameIndex">获取指定的堆栈帧</param>
        private void WriteLog(string messagestr, string infoDescribe, LogType logType, bool isWriteClassInfo = false, int frameIndex = 2)
        {
            lock (_writeLogLockObj)
            {
                StreamWriter streamWriter = null;
                try
                {
                    streamWriter = FileOpen(logType);
                    DateTime dateNow = DateTime.Now;
                    if (streamWriter != null)
                    {
                        //streamWriter.WriteLine("***********************************************************************");
                        streamWriter.WriteLine(dateNow.ToString("yyyy-MM-dd HH:mm:ss"));
                        //streamWriter.WriteLine("信息类型：" + infoDescribe);
                        if (isWriteClassInfo)
                        {
                            //index:0为本身的方法；1为调用方法；2为其上上层，依次类推
                            MethodBase mb = new StackTrace(true).GetFrame(frameIndex).GetMethod();

                            streamWriter.WriteLine("模块：" + mb.Module);
                            //if (mb.DeclaringType != null)
                            //{
                            //    streamWriter.WriteLine("命名空间名：" + mb.DeclaringType.Namespace);
                            //    //仅有类名
                            //    streamWriter.WriteLine("类名：" + mb.DeclaringType.Name);
                            //}
                            if (mb.ReflectedType != null)
                            {
                                //完整类名，包括命名空间
                                streamWriter.WriteLine("类名：" + mb.ReflectedType.FullName);
                                streamWriter.WriteLine("方法：" + mb.Name);
                            }
                        }
                        if (messagestr != null)
                        {
                            //streamWriter.WriteLine("信息内容：\r\n" + messagestr + "\r\n");
                            streamWriter.WriteLine(messagestr + "\r\n");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    if (streamWriter != null)
                    {
                        streamWriter.Flush();
                        streamWriter.Close();
                        streamWriter.Dispose();
                    }
                }
            }
        }
        #endregion

        #endregion

    }

    public class LogCongig
    {
        /// <summary>
        /// 是否写日志
        /// </summary>
        public bool LogIsWrite { get; set; } = false;
        /// <summary>
        /// 一般性日志输出
        /// </summary>
        public bool LogIsWriteByNormal { get; set; } = false;
        /// <summary>
        /// 调试日志输出
        /// </summary>
        public bool LogIsWriteByDebug { get; set; } = false;
        /// <summary>
        /// 错误日志输出
        /// </summary>
        public bool LogIsWriteByError { get; set; } = false;
        /// <summary>
        /// 日志保存路径
        /// </summary>
        public string LogFilePath { get; set; }

    }
}
