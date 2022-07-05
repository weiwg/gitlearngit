using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Model.Order;

namespace LY.Report.Core.Service.Order.Evaluation.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class OrderEvaluationGetInput 
    {
        /// <summary>
        /// 订单号
        /// </summary>
        [Required(ErrorMessage = "请输入订单号")]
        public string OrderNo { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 司机ID
        /// </summary>
        public string DriverId { get; set; }
    }
}
