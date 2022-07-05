
namespace LY.Report.Core.LYApiUtil.Pay
{
    /// <summary>
    /// Api接口消息
    /// </summary>
    public class PayApiResult
    {
        /// <summary>
        /// 是否成功标记
        /// </summary>
        public bool Success => Code == 1;

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 消息状态码
        /// </summary>
        public string MsgCode { get; set; }
    }

    /// <summary>
    /// Api接口实体
    /// </summary>
    public class PayApiResult<T> : PayApiResult
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }
}
