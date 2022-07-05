using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.Order.Enum;

namespace LY.Report.Core.Service.Order.Info.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class OrderInfoAddFullInput : OrderInfoAddInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage = "用户Id不能为空")]
        public string UserId { get; set; }

        /// <summary>
        /// 司机Id
        /// </summary>
        public string DriverId { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        [Required(ErrorMessage = "请选择订单类型")]
        public OrderType OrderType { get; set; }

        /// <summary>
        /// 外部订单编号
        /// </summary>
        [Required(ErrorMessage = "外部订单编号不能为空")]
        public string OutsideOrderNo { get; set; }

    }
}
