using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Order.Enum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Order
{
    /// <summary>
    /// 抵扣明细表
    /// </summary>
    [Table(Name = "T_Order_Deduction")]
    [Index("idx_{tablename}_01", nameof(DeductionId), true)]
    public class OrderDeduction : EntityTenantFull
    {
        /// <summary>
        /// 抵扣Id
        /// </summary>
        [Description("抵扣Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string DeductionId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [Description("订单号")]
        [Column(StringLength = 32, IsNullable = false)]
        public string OrderNo { get; set; }

        /// <summary>
        /// 抵扣类别(1优惠券 2积分 3红包)
        /// </summary>
        [Description("抵扣类别")]
        public DeductionType DeductionType { get; set; }

        /// <summary>
        /// 抵扣金额
        /// </summary>
        [Description("抵扣金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal DeductionAmount { get; set; }

        /// <summary>
        /// 已退款金额
        /// </summary>
        [Description("已退款金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal RefundedAmount { get; set; }

        /// <summary>
        /// 优惠券Id
        /// </summary>
        [Description("优惠券Id")]
        [Column(StringLength = 36)]
        public string CouponId { get; set; }

        /// <summary>
        /// 红包Id
        /// </summary>
        [Description("红包Id")]
        [Column(StringLength = 36)]
        public string RedPackId { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        [Description("积分")]
        public int IntegralAmount { get; set; }
    }
}
