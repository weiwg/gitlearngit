using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Order.Evaluation.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class OrderEvaluationAddInput
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Display(Name = "订单号")]
        [Required(ErrorMessage = "订单号不能为空！")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        [Display(Name = "评分")]
        [Required(ErrorMessage = "评分不能为空！")]
        public int Score { get; set; }

        /// <summary>
        /// 评价内容
        /// </summary>
        [Display(Name = "评价内容")]
        [Required(ErrorMessage = "评价内容不能为空！"), StringLength(150, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string Content { get; set; }

    }
}
