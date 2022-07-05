using System.ComponentModel;
using FreeSql.DataAnnotations;
using LY.Report.Core.Common.BaseModel;

namespace LY.Report.Core.Model.Fund
{
    /// <summary>
    /// 提现记录
    /// </summary>
    [Table(Name = "T_Fund_WithdrawRecord")]
    [Index("idx_{tablename}_01", nameof(RecordId), true)]
    public class FundWithdrawRecord : EntityTenantFull
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        [Description("记录Id")]
        [Column(IsPrimary = true, Position = 2, StringLength = 36, IsNullable = false)]
        public string RecordId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Description("用户Id")]
        [Column(StringLength = 36, IsNullable = false)]
        public string UserId { get; set; }
        
        /// <summary>
        /// 商户提现单号
        /// </summary>
        [Description("商户提现单号")]
        [Column(StringLength = 64, IsNullable = false)]
        public string WithdrawOutTradeNo { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        [Column(StringLength = 100)]
        public string Remark { get; set; }

    }
}
