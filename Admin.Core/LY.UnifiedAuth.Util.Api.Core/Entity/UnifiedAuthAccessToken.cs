/* ******************************************************
 * 作者：weig
 * 功能：API请求微信AccessToken实体
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20191024 weigang  创建   
 ***************************************************** */

using System;

namespace LY.UnifiedAuth.Util.Api.Core.Entity
{
    /// <summary>
    /// API请求微信AccessToken实体
    /// </summary>
    [Serializable]
    public class UnifiedAuthAccessToken
    {
        /// <summary>
        /// Api的Url
        /// </summary>
        public string ApiUrl { get; set; }
        /// <summary>
        /// AppId
        /// </summary>
        public string ApiAppId { get; set; }
        /// <summary>
        /// AppSecret
        /// </summary>
        public string ApiAppSecret { get; set; }
    }
}
