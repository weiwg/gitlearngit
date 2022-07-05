
using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.Delivery.Enum;

namespace LY.Report.Core.Business.DeliveryPrice.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class DeliveryPriceGetPriceIn
    {
        /// <summary>
        /// 车型Id
        /// </summary>
        [Required(ErrorMessage = "请选择车型")]
        public string CarId { get; set; }

        /// <summary>
        /// 计价类型
        /// </summary>
        [Required(ErrorMessage = "请选择计价类型")]
        public CalcRuleType CalcRuleType { get; set; }

        /// <summary>
        /// 条件 距离/体积/面积
        /// </summary>
        [Required(ErrorMessage = "请输入装载数量")]
        public double LoadCount { get; set; }

        /// <summary>
        /// 起始地点坐标(经度,纬度)
        /// </summary>
        [Required(ErrorMessage = "请选择起始地点")]
        public string StartCoordinate { get; set; }

        /// <summary>
        /// 途径地点坐标包括终点(经度,纬度,";"分隔)
        /// </summary>
        [Required(ErrorMessage = "途径地点坐标")]
        public string WayCoordinates { get; set; }
    }
}
