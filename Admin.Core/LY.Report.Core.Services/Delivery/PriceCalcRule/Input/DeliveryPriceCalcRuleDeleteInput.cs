
using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Delivery.PriceCalcRule.Input
{
    /// <summary>
    /// 删除
    /// </summary>
    public class DeliveryPriceCalcRuleDeleteInput
    {
        /// <summary>
        /// 规则Id
        /// </summary>
        [Required(ErrorMessage = "规则Id不能为空")]
        public string PriceRuleId { get; set; }
    }
}
