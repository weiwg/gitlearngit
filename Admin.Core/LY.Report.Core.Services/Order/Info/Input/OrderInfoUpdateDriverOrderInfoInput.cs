using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Order.Info.Input
{
    /// <summary>
    /// 订单信息修改
    /// </summary>
    public partial class OrderInfoUpdateDriverOrderInfoInput
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Required(ErrorMessage = "订单号不能为空！")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 司机备注
        /// </summary>
        [Required(ErrorMessage = "司机备注不能为空！")]
        public string DriverRemark { get; set; }
    }
}
