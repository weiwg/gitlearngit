using System;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Delivery.Enum;
using LY.Report.Core.Model.Order.Enum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Order
{
    /// <summary>
    /// 订单表
    /// </summary>
    [Table(Name = "T_Order_Info")]
    [Index("idx_{tablename}_01", nameof(OrderNo), true)]
    public class OrderInfo : EntityTenantFull
    {
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
        /// 车型名称
        /// </summary>
        [Description("车型名称")]
        [Column(StringLength = 50, IsNullable = false)]
        public string CarName { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Description("用户Id")]
        [Column(StringLength = 36, IsNullable = false)]
        public string UserId { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        [Description("订单类型")]
        public OrderType OrderType { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        [Description("订单状态")]
        public OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 用户是否评价
        /// </summary>
        [Description("用户是否评价")]
        public bool IsUserEvaluation { get; set; }

        /// <summary>
        /// 司机是否评价
        /// </summary>
        [Description("司机是否评价")]
        public bool IsDriverEvaluation { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        [Description("下单时间")]
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 接单时间
        /// </summary>
        [Description("接单时间")]
        public DateTime? ReceivedOrderDate { get; set; }

        /// <summary>
        /// 送货时间
        /// </summary>
        [Description("送货时间")]
        public DateTime? DeliveringOrderDate { get; set; }
        
        /// <summary>
        /// 送达时间
        /// </summary>
        [Description("送达时间")]
        public DateTime? DeliveredOrderDate { get; set; }

        /// <summary>
        /// 预计送达时间
        /// </summary>
        [Description("预计送达时间")]
        public DateTime? PlanDeliveredOrderDate { get; set; }

        /// <summary>
        /// 用户是否确认送达
        /// </summary>
        [Description("用户是否确认送达")]
        public bool IsUserConfirm { get; set; }

        /// <summary>
        /// 确认送达时间
        /// </summary>
        [Description("确认送达时间")]
        public DateTime? ConfirmOrderDate { get; set; }

        /// <summary>
        /// 取消状态
        /// </summary>
        [Description("取消状态")]
        public CancelStatus CancelStatus { get; set; }

        /// <summary>
        /// 取消时间
        /// </summary>
        [Description("取消时间")]
        public DateTime? CancelDate { get; set; }

        /// <summary>
        /// 取消原因
        /// </summary>
        [Description("取消原因")]
        [Column(StringLength = 100)]
        public string CancelReason { get; set; }

        /// <summary>
        /// 订单描述
        /// </summary>
        [Description("订单描述")]
        [Column(StringLength = 100, IsNullable = false)]
        public string OrderDescription { get; set; }

        /// <summary>
        /// 下单车型Id
        /// </summary>
        [Description("下单车型Id")]
        [Column(StringLength = 50, IsNullable = false)]
        public string CarId { get; set; }

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
        public CalcRuleType GoodsCalcRuleType { get; set; }

        /// <summary>
        /// 物品数量
        /// </summary>
        [Description("物品数量")]
        [Column(StringLength = 100, IsNullable = false)]
        public string GoodsLoadCount { get; set; }

        /// <summary>
        /// 基础运费(元)
        /// </summary>
        [Description("基础运费(元)")]
        [Column(Precision = 12, Scale = 2)]
        public decimal BaseFreight { get; set; }

        /// <summary>
        /// 距离运费(元)
        /// </summary>
        [Description("距离运费(元)")]
        [Column(Precision = 12, Scale = 2)]
        public decimal DistanceFreight { get; set; }

        /// <summary>
        /// 装载运费(元)
        /// </summary>
        [Description("装载运费(元)")]
        [Column(Precision = 12, Scale = 2)]
        public decimal LoadCountFreight { get; set; }

        /// <summary>
        /// 订单总运费(元)
        /// </summary>
        [Description("订单总运费(元)")]
        [Column(Precision = 12, Scale = 2)]
        public decimal TotalFreight { get; set; }

        /// <summary>
        /// 用户小费金额(元)
        /// </summary>
        [Description("用户小费金额(元)")]
        [Column(Precision = 12, Scale = 2)]
        public decimal UserTipsAmount { get; set; }

        /// <summary>
        /// 改价金额, 可正可负(正为加价,负为减价)
        /// </summary>
        [Description("改价金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal ChangeAmount { get; set; }

        /// <summary>
        /// 系统优惠金额
        /// </summary>
        [Description("系统优惠金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal SystemDiscountAmount { get; set; }

        /// <summary>
        /// 优惠券抵扣金额
        /// </summary>
        [Description("优惠券抵扣金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal CouponDiscountAmount { get; set; }

        /// <summary>
        /// 红包抵扣金额
        /// </summary>
        [Description("红包抵扣金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal RedPackDiscountAmount { get; set; }

        /// <summary>
        /// 订单总金额, 订单总运费+用户小费+改价
        /// </summary>
        [Description("订单总金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal TotalAmount { get; set; }
        //=>TotalFreight + UserTipsAmount + ChangeAmount;

        /// <summary>
        /// 结算手续费(交易服务费)
        /// </summary>
        [Description("结算手续费")]
        [Column(Precision = 12, Scale = 2)]
        public decimal SettleCharge { get; set; }

        /// <summary>
        /// 结算总金额(结算给司机)
        /// </summary>
        [Description("结算总金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal SettleTotalAmount { get; set; }
        //=>TotalAmount - SettleCharge;

        /// <summary>
        /// 应付金额(用户), 订单总金额-系统优惠金额-优惠券-红包
        /// </summary>
        [Description("应付金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal AmountPayable { get; set; }
        //=>TotalAmount - SystemDiscountAmount - CouponDiscountAmount - RedPackDiscountAmount;

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
        /// 起始地点坐标(经度)
        /// </summary>
        [Description("起始地点坐标")]
        public double StartLongitude { get; set; }

        /// <summary>
        /// 起始地点坐标(纬度)
        /// </summary>
        [Description("起始地点坐标")]
        public double StartLatitude { get; set; }

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
        /// 地址距离km
        /// </summary>
        [Description("地址距离km")]
        [Column(StringLength = 100, IsNullable = false)]
        public string AddressDistance { get; set; }

        /// <summary>
        /// 途经点数量
        /// </summary>
        [Description("途经点数量")]
        [Column(IsNullable = false)]
        public int DeliveryWayCount { get; set; }

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
        /// 获取系统补贴金额
        /// 系统优惠+优惠券+红包抵扣
        /// </summary>
        /// <returns></returns>
        public decimal GetAppSubsidyAmount() => SystemDiscountAmount + CouponDiscountAmount + RedPackDiscountAmount;

        /// <summary>
        /// 获取订单总金额(不含红包抵扣)
        /// 订单总运费+用户小费+改价
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalAmount() => TotalFreight + UserTipsAmount + ChangeAmount;

        /// <summary>
        /// 获取结算金额
        /// 订单总金额-结算手续费
        /// </summary>
        /// <returns></returns>
        public decimal GetSettleTotalAmount() => GetTotalAmount() - SettleCharge;

        /// <summary>
        /// 获取应付金额
        /// 订单总金额-系统补贴优惠(系统优惠金额-优惠券-红包抵扣)
        /// </summary>
        /// <returns></returns>
        public decimal GetAmountPayable() => GetTotalAmount() - GetAppSubsidyAmount();
    }
}
