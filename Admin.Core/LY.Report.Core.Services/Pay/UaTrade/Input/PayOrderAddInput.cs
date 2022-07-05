
using LY.Report.Core.Model.Pay.Enum;
using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Pay.UaTrade.Input
{
    /// <summary>
    /// 余额充值
    /// </summary>
    public class PayOrderAddInput
    {
        /// <summary>
        /// 商户单号
        /// </summary>
        [Display(Name = "商户单号")]
        [Required(ErrorMessage = "商户单号不能为空！")]
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 是否使用红包
        /// </summary>
        [Display(Name = "是否使用红包")]
        public bool IsUseRedPack { get; set; }

        /// <summary>
        /// 优惠券记录Id
        /// </summary>
        [Display(Name = "优惠券Id")]
        public string CouponRecordId { get; set; }

        /// <summary>
        /// 支付平台
        /// </summary>
        public FundPlatform PayPlatform { get; set; }
    }
}
