/* ******************************************************
 * 版权：广东易昂普软件信息有限公司
 * 作者：卢志成
 * 功能：日志帮助类
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20190111 luzhicheng  创建   
 ***************************************************** */

namespace LY.Report.Core.Util.Common
{
    /// <summary>
    ///  日志帮助接口
    /// </summary>
    public interface ILogHelper
    {
        #region 写入DEBUG日志

        /// <summary>
        /// 写入DEBUG日志
        /// </summary>
        /// <param name="msg">日志消息</param>
        void Debug(string msg);

        /// <summary>
        /// 写入DEBUG日志
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="msg">日志消息</param>
        void Debug(string className, string msg);

        /// <summary>
        /// 写入DEBUG日志
        /// </summary>
        /// <param name="msg">日志消息</param>
        /// <param name="isWriteClassInfo">是否写入调用类信息</param>
        /// <param name="frameIndex">获取指定的堆栈帧</param>
        void Debug(string msg, bool isWriteClassInfo, int frameIndex = 1);
        #endregion

        #region 写入普通日志

        /// <summary>
        /// 写入普通日志
        /// </summary>
        /// <param name="msg">日志消息</param>
        void Info(string msg);

        /// <summary>
        /// 写入普通日志
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="msg">日志消息</param>
        void Info(string className, string msg);

        /// <summary>
        /// 写入普通日志
        /// </summary>
        /// <param name="msg">日志消息</param>
        /// <param name="isWriteClassInfo">是否写入调用类信息</param>
        /// <param name="frameIndex">获取指定的堆栈帧</param>
        void Info(string msg, bool isWriteClassInfo, int frameIndex = 1);
        #endregion

        #region 写入错误日志

        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="msg">日志消息</param>
        void Error(string msg);

        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="msg">日志消息</param>
        void Error(string className, string msg);

        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="msg">日志消息</param>
        /// <param name="isWriteClassInfo">是否写入调用类信息</param>
        /// <param name="frameIndex">获取指定的堆栈帧</param>
        void Error(string msg, bool isWriteClassInfo, int frameIndex = 1);
        #endregion

    }
}
