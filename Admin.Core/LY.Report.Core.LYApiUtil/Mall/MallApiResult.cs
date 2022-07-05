using System.Collections;
using System.Collections.Generic;

namespace LY.Report.Core.LYApiUtil.Mall
{
    /// <summary>
    /// 商城接口实体
    /// </summary>
    public class MallApiResultBase
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
        /// 消息状态码
        /// </summary>
        public string ResultCode { get; set; }
    }

    /// <summary>
    /// 商城接口实体
    /// </summary>
    public class MallApiResult : MallApiResultBase
    {
        /// <summary>
        /// 数据
        /// </summary>
        public Hashtable Data { get; set; }
    }

    /// <summary>
    /// 商城接口实体
    /// </summary>
    public class MallApiResultList : MallApiResultBase
    {
        /// <summary>
        /// 数据
        /// </summary>
        public List<Hashtable> DataList { get; set; }
    }
}
