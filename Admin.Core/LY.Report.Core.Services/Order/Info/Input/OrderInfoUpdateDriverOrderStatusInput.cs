using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Order.Info.Input
{
    /// <summary>
    /// 修改司机订单状态
    /// </summary>
    public partial class OrderInfoUpdateDriverOrderStatusInput
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Required(ErrorMessage = "订单号不能为空！")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 配送序号
        /// </summary>
        [Required(ErrorMessage = "配送序号不能为空！")]
        public string DeliveryNo { get; set; }

        ///// <summary>
        ///// 司机Id
        ///// </summary>
        //public string DriverId { get; set; }
    }
}
