using System;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Delivery.Enum;
using LY.Report.Core.Model.Order.Enum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Order
{
    /// <summary>
    /// 订单配送信息表
    /// </summary>
    [Table(Name = "T_Order_Delivery")]
    [Index("idx_{tablename}_01", "DeliveryNo AES, OrderNo AES", true)]
    public class OrderDelivery : EntityTenantFull
    {
        /// <summary>
        /// 配送序号
        /// </summary>
        [Description("配送序号")]
        [Column(IsPrimary = true, StringLength = 40, IsNullable = false)]
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [Description("订单号")]
        [Column(IsPrimary = true, StringLength = 32, IsNullable = false)]
        public string OrderNo { get; set; }

        /// <summary>
        /// 商户单号
        /// </summary>
        [Description("商户单号")]
        [Column(StringLength = 32, IsNullable = false)]
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 外部订单号
        /// </summary>
        [Description("外部订单号")]
        [Column(StringLength = 50)]
        public string OutsideOrderNo { get; set; }

        /// <summary>
        /// 司机Id
        /// </summary>
        [Description("司机Id")]
        [Column(StringLength = 36)]
        public string DriverId { get; set; }

        /// <summary>
        /// 车牌号码
        /// </summary>
        [Description("车牌号码")]
        [Column(StringLength = 10, IsNullable = false)]
        public string CarLicensePlate { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Description("用户Id")]
        [Column(StringLength = 36, IsNullable = false)]
        public string UserId { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        [Description("订单状态")]
        public OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 送达时间
        /// </summary>
        [Description("送达时间")]
        public DateTime? DeliveredOrderDate { get; set; }

        /// <summary>
        /// 物品信息
        /// </summary>
        [Description("物品信息")]
        [Column(StringLength = 100, IsNullable = false)]
        public string GoodsInfo { get; set; }

        /// <summary>
        /// 物品计价类型
        /// </summary>
        [Description("物品计价类型")]
        [Column(IsNullable = false)]
        public CalcRuleType GoodsCalcRuleType { get; set; }

        /// <summary>
        /// 物品数量
        /// </summary>
        [Description("物品数量")]
        [Column(StringLength = 100, IsNullable = false)]
        public string GoodsLoadCount { get; set; }

        /// <summary>
        /// 起始基本地址(省|市|区)
        /// </summary>
        [Description("起始基本地址")]
        [Column(StringLength = 50, IsNullable = false)]
        public string StartBaseAddress { get; set; }

        /// <summary>
        /// 起始地址详情
        /// </summary>
        [Description("起始地址详情")]
        [Column(StringLength = 100, IsNullable = false)]
        public string StartDetailAddress { get; set; }

        /// <summary>
        /// 起始地址名称
        /// </summary>
        [Description("起始地址名称")]
        [Column(StringLength = 100, IsNullable = false)]
        public string StartAddressName { get; set; }

        /// <summary>
        /// 起始地点坐标(经度,纬度)
        /// </summary>
        [Description("起始地点坐标")]
        [Column(StringLength = 100, IsNullable = false)]
        public string StartCoordinate { get; set; }

        /// <summary>
        /// 终点基本地址(省|市|区)
        /// </summary>
        [Description("终点基本地址")]
        [Column(StringLength = 50, IsNullable = false)]
        public string EndBaseAddress { get; set; }

        /// <summary>
        /// 终点地址详情
        /// </summary>
        [Description("终点地址详情")]
        [Column(StringLength = 100, IsNullable = false)]
        public string EndDetailAddress { get; set; }

        /// <summary>
        /// 终点地址名称
        /// </summary>
        [Description("终点地址名称")]
        [Column(StringLength = 100, IsNullable = false)]
        public string EndAddressName { get; set; }

        /// <summary>
        /// 终点地点坐标(经度,纬度)
        /// </summary>
        [Description("终点地点坐标")]
        [Column(StringLength = 100, IsNullable = false)]
        public string EndCoordinate { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        [Description("收货人姓名")]
        [Column(StringLength = 100, IsNullable = false)]
        public string ConsigneeName { get; set; }

        /// <summary>
        /// 收货手机号码
        /// </summary>
        [Description("收货手机号码")]
        [Column(StringLength = 100, IsNullable = false)]
        public string ConsigneePhone { get; set; }

        /// <summary>
        /// 用户备注
        /// </summary>
        [Description("用户备注")]
        [Column(StringLength = 100)]
        public string UserRemark { get; set; }

        /// <summary>
        /// 司机备注
        /// </summary>
        [Description("司机备注")]
        [Column(StringLength = 100)]
        public string DriverRemark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Description("排序")]
        public int Sort { get; set; }

    }
}
