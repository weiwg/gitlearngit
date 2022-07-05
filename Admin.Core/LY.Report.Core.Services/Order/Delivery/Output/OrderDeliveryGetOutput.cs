using System;
using LY.Report.Core.Model.Delivery.Enum;
using LY.Report.Core.Model.Order.Enum;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Service.Order.Delivery.Output
{
    public class OrderDeliveryGetOutput
    {
        /// <summary>
        /// 配送序号
        /// </summary>
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 订单状态描述
        /// </summary>
        public string OrderStatusDescribe => EnumHelper.GetDescription(OrderStatus);

        /// <summary>
        /// 送达时间
        /// </summary>
        public DateTime? DeliveredOrderDate { get; set; }

        /// <summary>
        /// 物品信息
        /// </summary>
        public string GoodsInfo { get; set; }

        /// <summary>
        /// 物品计价类型
        /// </summary>
        public CalcRuleType GoodsCalcRuleType { get; set; }

        /// <summary>
        /// 物品计价类型描述
        /// </summary>
        public string GoodsCalcRuleTypeDescribe => EnumHelper.GetDescription(GoodsCalcRuleType);

        /// <summary>
        /// 物品数量
        /// </summary>
        public string GoodsLoadCount { get; set; }

        /// <summary>
        /// 起始基本地址(省|市|区)
        /// </summary>
        public string StartBaseAddress { get; set; }

        /// <summary>
        /// 起始地址详情
        /// </summary>
        public string StartDetailAddress { get; set; }

        /// <summary>
        /// 起始地址名称
        /// </summary>
        public string StartAddressName { get; set; }

        /// <summary>
        /// 起始地点坐标(经度,纬度)
        /// </summary>
        public string StartCoordinate { get; set; }

        /// <summary>
        /// 终点基本地址(省|市|区)
        /// </summary>
        public string EndBaseAddress { get; set; }

        /// <summary>
        /// 终点地址详情
        /// </summary>
        public string EndDetailAddress { get; set; }

        /// <summary>
        /// 终点地址名称
        /// </summary>
        public string EndAddressName { get; set; }

        /// <summary>
        /// 终点地点坐标(经度,纬度)
        /// </summary>
        public string EndCoordinate { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ConsigneeName { get; set; }

        /// <summary>
        /// 收货手机号码
        /// </summary>
        public string ConsigneePhone { get; set; }

        /// <summary>
        /// 用户备注
        /// </summary>
        public string UserRemark { get; set; }

        /// <summary>
        /// 司机备注
        /// </summary>
        public string DriverRemark { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Sort { get; set; }
    }
}
