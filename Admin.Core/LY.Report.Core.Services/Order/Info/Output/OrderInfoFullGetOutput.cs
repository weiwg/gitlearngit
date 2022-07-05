using System.Collections.Generic;
using LY.Report.Core.Service.Order.Delivery.Output;

namespace LY.Report.Core.Service.Order.Info.Output
{
    public class OrderInfoFullGetOutput : OrderInfoGetOutput
    {
        /// <summary>
        /// 途径地点坐标(经度,纬度)
        /// </summary>
        public List<OrderDeliveryListOutput> WayCoordinates { get; set; } = new List<OrderDeliveryListOutput>();
    }
}
