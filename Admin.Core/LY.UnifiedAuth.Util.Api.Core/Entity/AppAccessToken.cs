/* ******************************************************
 * 作者：weig
 * 功能：Api令牌实体
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20191024 weigang  创建   
 ***************************************************** */

using System;

namespace LY.UnifiedAuth.Util.Api.Core.Entity
{
    /// <summary>
    /// Api令牌实体
    /// </summary>
    [Serializable]
    public class AppAccessToken
    {
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// AppSecret
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// AccessToken
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// SysAuthToken
        /// </summary>
        public string SysAuthToken { get; set; }
        /// <summary>
        /// SysAuthJwtToken
        /// </summary>
        public string SysAuthJwtToken { get; set; }
        /// <summary>
        /// 有效时间(秒)
        /// </summary>
        public int ExpiresIn { get; set; }
        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime ExpiresDate { get; set; }
        /// <summary>
        /// 时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
        /// </summary>
        public long Timestamp { get; set; }
    }
}
