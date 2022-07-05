
using System.ComponentModel.DataAnnotations;
using LY.Report.Core.Util.Attributes.Validation;

namespace LY.Report.Core.Service.Pay.UaTrade.Input
{
    /// <summary>
    /// 余额充值
    /// </summary>
    public class RechargeAddInput
    {
        /// <summary>
        /// 充值金额
        /// </summary>
        [Display(Name = "充值金额")]
        [CustomPrice]
        public decimal RechargeAmount { get; set; }
    }
}
