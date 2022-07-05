using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Order.Info.Input
{
    /// <summary>
    /// 司机接单
    /// </summary>
    public partial class OrderInfoUpdateDriverReceiveInput
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Required(ErrorMessage = "订单号不能为空！")]
        public string OrderNo { get; set; }
        
        ///// <summary>
        ///// 司机Id
        ///// </summary>
        //public string DriverId { get; set; }
    }
}
