using LY.Report.Core.Model.Delivery.Enum;

namespace LY.Report.Core.Service.Order.FreightCalc.Output
{
    public class OrderFreghtCalcGetOutput
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        ///车型Id
        /// </summary>
        public string CarId { get; set; }

        /// <summary>
        ///车型名称
        /// </summary>]
        public string CarName { get; set; }

        /// <summary>
        /// 计价类型
        /// </summary>
        public CalcRuleType CalcRuleType { get; set; }

        /// <summary>
        /// 条件 距离/体积/面积
        /// </summary>
        public double LoadCount { get; set; }

        /// <summary>
        /// 起始地点坐标(经度,纬度)
        /// </summary>
        public string StartCoordinate { get; set; }

        /// <summary>
        /// 途径地点坐标包括终点(经度,纬度,";"分隔)
        /// </summary>
        public string WayCoordinates { get; set; }

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
        /// 订单总运费(元)
        /// </summary>
        public decimal TotalFreight { get; set; }

        /// <summary>
        /// 用户小费金额(元)
        /// </summary>
        public decimal UserTipsAmount { get; set; }

        /// <summary>
        /// 距离(千米)
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// 耗时(分钟),向上取整
        /// </summary>
        public double Duration { get; set; }

        /// <summary>
        /// 取价城市
        /// </summary>
        public string PriceCity { get; set; }
    }
}

