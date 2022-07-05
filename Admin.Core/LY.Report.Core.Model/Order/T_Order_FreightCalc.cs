using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Delivery.Enum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Order
{
    /// <summary>
    /// 订单运费计价
    /// </summary>
    [Table(Name = "T_Order_FreightCalc")]
    [Index("idx_{tablename}_01", nameof(CalcId), true)]
    public class OrderFreightCalc : EntityTenantFull
    {
        /// <summary>
        /// 计价Id
        /// </summary>
        [Description("计价Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string CalcId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [Description("订单号")]
        [Column(StringLength = 36, IsNullable = false)]
        public string OrderNo { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Description("用户ID")]
        [Column(StringLength = 36, IsNullable = false)]
        public string UserId { get; set; }

        /// <summary>
        ///司机ID
        /// </summary>
        [Description("司机ID")]
        [Column(StringLength = 36, IsNullable = false)]
        public string DriverId { get; set; }

        /// <summary>
        ///车型Id
        /// </summary>
        [Description("车型Id")]
        [Column(StringLength = 36, IsNullable = false)]
        public string CarId { get; set; }

        /// <summary>
        ///车型名称
        /// </summary>
        [Description("车型名称")]
        [Column(StringLength = 50, IsNullable = false)]
        public string CarName { get; set; }

        /// <summary>
        /// 计价类型
        /// </summary>
        [Description("计价类型")]
        public CalcRuleType CalcRuleType { get; set; }

        /// <summary>
        /// 条件 距离/体积/面积
        /// </summary>
        [Description("条件")]
        public double LoadCount { get; set; }

        /// <summary>
        /// 起始地点坐标(经度,纬度)
        /// </summary>
        [Description("起始地点坐标")]
        [Column(StringLength = -1, IsNullable = false)]
        public string StartCoordinate { get; set; }

        /// <summary>
        /// 途径地点坐标包括终点(经度,纬度,";"分隔)
        /// </summary>
        [Description("途径地点坐标")]
        [Column(StringLength = -1, IsNullable = false)]
        public string WayCoordinates { get; set; }

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
        /// 距离(千米)
        /// </summary>
        [Description("距离(千米)")]
        public double Distance { get; set; }

        /// <summary>
        /// 耗时(分钟),向上取整
        /// </summary>
        [Description("耗时(分钟)")]
        public double Duration { get; set; }

        /// <summary>
        /// 取价城市
        /// </summary>
        [Description("取价城市")]
        [Column(StringLength = 50, IsNullable = false)]
        public string PriceCity { get; set; }
    }
}
