using LY.Report.Core.Model.Order.Enum;
using System;

namespace LY.Report.Core.Service.Order.Info.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class OrderInfoGetInput
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 商户单号
        /// </summary>
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 外部订单号
        /// </summary>
        public string OutsideOrderNo { get; set; }

        /// <summary>
        /// 司机Id
        /// </summary>
        public string DriverId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 取消状态
        /// </summary>
        public CancelStatus CancelStatus { get; set; }

        /// <summary>
        /// 订单描述
        /// </summary>
        public string OrderDescription { get; set; }

        /// <summary>
        /// 开始下单时间
        /// </summary>
        public string OrderStartDate { get; set; }
        /// <summary>
        /// 结束下单时间
        /// </summary>
        public string OrderEndDate { get; set; }
    }
}
