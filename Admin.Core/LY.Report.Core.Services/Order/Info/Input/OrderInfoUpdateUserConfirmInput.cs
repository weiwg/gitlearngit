using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Order.Info.Input
{
    /// <summary>
    /// 修改用户订单确认
    /// </summary>
    public partial class OrderInfoUpdateUserConfirmInput
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Required(ErrorMessage = "订单号不能为空！")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [Required(ErrorMessage = "新密码不能为空！"), StringLength(32, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 32)]
        public string PayPassword { get; set; }
    }
}
