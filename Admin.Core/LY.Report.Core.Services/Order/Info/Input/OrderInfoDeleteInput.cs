using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Order.Info.Input
{
    /// <summary>
    /// 删除
    /// </summary>
    public class OrderInfoDeleteInput
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        [Required(ErrorMessage = "订单号不能为空！")]
        public string OrderNo { get; set; }
    }
}
