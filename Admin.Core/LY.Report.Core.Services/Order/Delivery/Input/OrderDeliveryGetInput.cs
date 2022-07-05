using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.Order;

namespace LY.Report.Core.Service.Order.Delivery.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class OrderDeliveryGetInput
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Required(ErrorMessage = "请输入订单号")]
        public string OrderNo { get; set; }
    }
}
