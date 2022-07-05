using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Order.Info.Input
{
    /// <summary>
    /// 取消订单
    /// </summary>
    public partial class OrderInfoCancelOrderInput
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Required(ErrorMessage = "订单号不能为空！")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 取消原因
        /// </summary>
        [Required(ErrorMessage = "取消原因不能为空！")]
        public string CancelReason { get; set; }
    }
}
