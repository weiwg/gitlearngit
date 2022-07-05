
using System.Collections.Generic;

namespace LY.Report.Core.LYApiUtil.Pay.Out
{
    /// <summary>
    /// 分页查询
    /// </summary>
    public class PageOut<T>
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        public long Total { get; set; } = 0;

        /// <summary>
        /// 数据
        /// </summary>
        public IList<T> List { get; set; }
    }
}
