
using System;
using LY.Report.Core.Model.Delivery.Enum;
using LY.Report.Core.Model.Order.Enum;
using LY.Report.Core.Util.Tool;

namespace LY.Report.Core.Service.Order.Info.Output
{
    public class OrderInfoGetOutput
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
        /// 车牌号码
        /// </summary>
        public string CarLicensePlate { get; set; }

        /// <summary>
        /// 车型名称
        /// </summary>
        public string CarName { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// 订单类型描述
        /// </summary>
        public string OrderTypeDescribe => EnumHelper.GetDescription(OrderType);

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 订单状态描述
        /// </summary>
        public string OrderStatusDescribe => EnumHelper.GetDescription(OrderStatus);

        /// <summary>
        /// 用户是否评价
        /// </summary>
        public bool IsUserEvaluation { get; set; }

        /// <summary>
        /// 司机是否评价
        /// </summary>
        public bool IsDriverEvaluation { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 接单时间
        /// </summary>
        public DateTime? ReceivedOrderDate { get; set; }

        /// <summary>
        /// 送达时间
        /// </summary>
        public DateTime? DeliveredOrderDate { get; set; }

        /// <summary>
        /// 预计送达时间
        /// </summary>
        public DateTime? PlanDeliveredOrderDate { get; set; }

        /// <summary>
        /// 用户是否确认送达
        /// </summary>
        public bool IsUserConfirm { get; set; }

        /// <summary>
        /// 确认送达时间
        /// </summary>
        public DateTime? ConfirmOrderDate { get; set; }

        /// <summary>
        /// 取消状态
        /// </summary>
        public CancelStatus CancelStatus { get; set; }

        /// <summary>
        /// 取消状态描述
        /// </summary>
        public string CancelStatusDescribe => EnumHelper.GetDescription(CancelStatus);

        /// <summary>
        /// 取消时间
        /// </summary>
        public DateTime? CancelDate { get; set; }

        /// <summary>
        /// 取消原因
        /// </summary>
        public string CancelReason { get; set; }

        /// <summary>
        /// 订单描述
        /// </summary>
        public string OrderDescription { get; set; }

        /// <summary>
        /// 下单车型Id
        /// </summary>
        public string CarId { get; set; }

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
        /// 基础运费(元)
        /// </summary>
        public decimal BaseFreight { get; set; }

        /// <summary>
        /// 距离运费(元)
        /// </summary>
        public decimal DistanceFreight { get; set; }

        /// <summary>
        /// 装载运费(元)
        /// </summary>
        public decimal LoadCountFreight { get; set; }

        /// <summary>
        /// 用户小费金额(元)
        /// </summary>
        public decimal UserTipsAmount { get; set; }

        /// <summary>
        /// 订单总运费(元)
        /// </summary>
        public decimal TotalFreight { get; set; }

        /// <summary>
        /// 系统优惠金额
        /// </summary>
        public decimal SystemDiscountAmount { get; set; }

        /// <summary>
        /// 优惠券抵扣金额
        /// </summary>
        public decimal CouponDiscountAmount { get; set; }

        /// <summary>
        /// 红包抵扣金额
        /// </summary>
        public decimal RedPackDiscountAmount { get; set; }

        /// <summary>
        /// 改价金额, 可正可负(正为加价,负为减价)
        /// </summary>
        public decimal ChangeAmount { get; set; }

        /// <summary>
        /// 订单总金额, 订单总运费+用户小费-系统优惠金额-优惠券+改价
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 结算手续费(交易服务费)
        /// </summary>
        public decimal SettleCharge { get; set; }

        /// <summary>
        /// 结算总金额(结算给司机)
        /// </summary>
        public decimal SettleTotalAmount { get; set; }

        /// <summary>
        /// 应付金额(用户), 订单总金额-红包
        /// </summary>
        public decimal AmountPayable { get; set; }

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
        /// 地址距离km
        /// </summary>
        public string AddressDistance { get; set; }

        /// <summary>
        /// 途经点数量
        /// </summary>
        public int DeliveryWayCount { get; set; }

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
        /// 发单昵称
        /// </summary>
        public string UserNickName { get; set; }

        /// <summary>
        /// 发单头像
        /// </summary>
        public string UserPortrait { get; set; }

        /// <summary>
        /// 发单手机号
        /// </summary>
        public string UserPhone { get; set; }

        /// <summary>
        /// 司机昵称
        /// </summary>
        public string DriverNickName { get; set; }

        /// <summary>
        /// 司机头像
        /// </summary>
        public string DriverPortrait { get; set; }

        /// <summary>
        /// 司机手机号
        /// </summary>
        public string DriverPhone { get; set; }


    }
}
