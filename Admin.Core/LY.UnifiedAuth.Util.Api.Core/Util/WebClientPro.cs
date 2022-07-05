/* ******************************************************
 * 作者：weig
 * 功能：Aes加解密工具类
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20191101 weigang  创建 
 ***************************************************** */

using System;
using System.Net;

namespace LY.UnifiedAuth.Util.Api.Core.Util
{
    /// <summary>
    /// Provides common methods for sending data to and receiving data from a resource identified by a URI.
    /// override Timeout
    /// </summary>
    public class WebClientPro : WebClient
    {
        /// <summary>
        /// 请求超时设置（以毫秒为单位），默认为10秒。
        /// </summary>
        public int Timeout { get; set; } = 10000;

        /// <summary>
        /// 重写GetWebRequest,添加WebRequest对象超时时间
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            //WebClient里上传下载的方法很多，但最终应该都是调用了这个方法
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = Timeout;
            request.ReadWriteTimeout = Timeout;
            return request;
        }
    }
}