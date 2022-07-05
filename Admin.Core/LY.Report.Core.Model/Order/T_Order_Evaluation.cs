using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Order
{
    /// <summary>
    /// 订单评价
    /// </summary>
    [Table(Name = "T_Order_Evaluation")]
    [Index("idx_{tablename}_01", nameof(OrderNo), true)]
    public class OrderEvaluation : EntityTenantFull
    {
        /// <summary>
        /// 评价Id
        /// </summary>
        [Description("评价Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string EvaluationId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [Description("订单号")]
        [Column(StringLength = 32, IsNullable = false)]
        public string OrderNo { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Description("用户Id")]
        [Column(StringLength = 36, IsNullable = false)]
        public string UserId { get; set; }

        /// <summary>
        /// 司机Id
        /// </summary>
        [Description("司机Id")]
        [Column(StringLength = 36, IsNullable = false)]
        public string DriverId { get; set; }

        /// <summary>
        /// 用户评分(司机评价用户)
        /// </summary>
        [Description("用户评分")]
        public int UserScore { get; set; }

        /// <summary>
        ///  评价用户内容
        /// </summary>
        [Description("评价用户内容")]
        [Column(StringLength = 150, IsNullable = false)]
        public string UserEvaluationContent { get; set; }

        /// <summary>
        /// 司机评分(用户评价司机)
        /// </summary>
        [Description("司机评分")]
        public int DriverScore { get; set; }

        /// <summary>
        ///  评价司机内容
        /// </summary>
        [Description("评价司机内容")]
        [Column(StringLength = 150, IsNullable = false)]
        public string DriverEvaluationContent { get; set; }

    }
}
