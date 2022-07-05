/* ******************************************************
 * 作者：weig
 * 功能：Api令牌实体
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20191024 weig  创建   
 ***************************************************** */

using System;

namespace LY.UnifiedAuth.Util.Api.Core.Entity
{
    /// <summary>
    /// 登录token
    /// </summary>
    [Serializable]
    public class AppLoginToken
    {
        /// <summary>
        /// 当前SessionId
        /// </summary>
        public string UnifiedSessionId { set; get; }
        /// <summary>
        /// 企业编号
        /// </summary>
        public string ApplyId { get; set; }
        /// <summary>
        /// 全局用户ID
        /// </summary>
        public string UnifiedUserId { get; set; }
        /// <summary>
        /// 微信OpenId
        /// </summary>
        public string OpenId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
        /// </summary>
        public long Timestamp { get; set; }
        /// <summary>
        /// 登录方式
        /// </summary>
        public string LoginMode { get; set; }
    }
}
