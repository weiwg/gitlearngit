/* ******************************************************
 * 作者：weig
 * 功能：API消息接口
 *  日期     修改人     修改记录
  --------------------------------------------------
 * 20191024 weigang  创建   
 ***************************************************** */


namespace LY.UnifiedAuth.Util.Api.Core.Entity
{
    /// <summary>
    /// API消息接口
    /// </summary>
    public interface IApiResult
    {
        ///// <summary>
        ///// 原始请求消息
        ///// </summary>
        //bool OriMsg { get; set; }
        ///// <summary>
        ///// 原始请求状态
        ///// </summary>
        //bool OriStatus { get; set; }
        /// <summary>
        /// 请求原始数据
        /// </summary>
        string OriResult { get; }
    }
}