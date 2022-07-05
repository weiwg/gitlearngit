/* ******************************************************
 * 作者：weig
 * 功能：API消息实体
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20191024 weigang  创建   
 ***************************************************** */

using System;
using System.Collections;
using System.Collections.Generic;

namespace LY.UnifiedAuth.Util.Api.Core.Entity
{
    /// <summary>
    /// API消息实体
    /// </summary>
    [Serializable]
    public class ApiResult : IApiResult
    {
        /// <summary>
        /// 请求状态(不代表业务成功)
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 业务状态码
        /// </summary>
        public string ResultCode { get; set; }
        /// <summary>
        /// 业务数据
        /// </summary>
        public Hashtable Data { get; set; }
        /// <summary>
        /// 业务数据
        /// </summary>
        public List<Hashtable> DataList { get; set; }
        /// <summary>
        /// 原始数据
        /// </summary>
        public string OriResult { get; set; }
    }
}
