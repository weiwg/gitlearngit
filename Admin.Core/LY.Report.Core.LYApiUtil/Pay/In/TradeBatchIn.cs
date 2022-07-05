using System.Collections.Generic;

namespace LY.Report.Core.LYApiUtil.Pay.In
{
    /// <summary>
    /// 批量交易下单
    /// </summary>
    public class TradeBatchIn
    {
        /// <summary>
        /// 交易列表
        /// </summary>
        public List<TradeIn> Trade { get; set; }
    }
}
