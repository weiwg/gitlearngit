using LY.Report.Core.Model.Order.Enum;

namespace LY.Report.Core.Service.Order.Info.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class OrderInfoGetWaitingOrderInput
    {
        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType OrderType { get; set; }
    }
}
