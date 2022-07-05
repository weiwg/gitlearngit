using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Order.Info.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class OrderInfoUpdateInput
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Required(ErrorMessage = "订单号不能为空！")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public long Version { get; set; }
    }
}
