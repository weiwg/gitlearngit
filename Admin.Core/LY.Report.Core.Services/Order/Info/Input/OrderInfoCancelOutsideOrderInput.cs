using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Order.Info.Input
{
    /// <summary>
    /// 取消外部订单
    /// </summary>
    public partial class OrderInfoCancelOutsideOrderInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage = "用户Id不能为空！")]
        public string UserId { get; set; }

        /// <summary>
        /// 物流订单号
        /// </summary>
        [Required(ErrorMessage = "订单号不能为空！")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 外部订单号
        /// </summary>
        [Required(ErrorMessage = "外部订单号不能为空！")]
        public string OutsideOrderNo { get; set; }

        /// <summary>
        /// 取消原因
        /// </summary>
        [Required(ErrorMessage = "取消原因不能为空！")]
        public string CancelReason { get; set; }
        
        /// <summary>
        /// 是否退回货物(仅送货中有效)
        /// </summary>
        [Required(ErrorMessage = "请填写是否退回货物！")]
        public bool IsReturnBack { get; set; }
    }
}
