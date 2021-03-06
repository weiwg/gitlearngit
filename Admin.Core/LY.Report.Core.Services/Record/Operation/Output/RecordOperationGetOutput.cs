using System;

namespace LY.Report.Core.Service.Record.Operation.Output
{
    public class RecordOperationGetOutput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        public string ApiLabel { get; set; }

        /// <summary>
        /// 接口地址
        /// </summary>
        public string ApiPath { get; set; }

        /// <summary>
        /// 接口提交方法
        /// </summary>
        public string ApiMethod { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string Os { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// 耗时（毫秒）
        /// </summary>
        public long ElapsedMilliseconds { get; set; }

        /// <summary>
        /// 操作状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 操作消息
        /// </summary>
        public string Msg { get; set; }

    }
}
