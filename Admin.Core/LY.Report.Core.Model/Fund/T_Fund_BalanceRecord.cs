using System;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Fund.Enum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Fund
{
    /// <summary>
    /// 用户余额记录
    /// </summary>
    [Table(Name = "T_Fund_BalanceRecord")]
    [Index("idx_{tablename}_01", nameof(RecordId), true)]
    public class FundBalanceRecord : EntityTenantFull
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
        [Column(StringLength = 36, Position = 3, IsNullable = false)]
        public string UserId { get; set; }

        /// <summary>
        /// 资金记录类型
        /// </summary>
        [Description("资金记录类型")]
        public FundRecordType FundRecordType { get; set; }

        /// <summary>
        /// 资金类型
        /// </summary>
        [Description("资金类型")]
        public FundType FundType { get; set; }

        /// <summary>
        /// 变动余额
        /// </summary>
        [Description("变动余额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal ChangeAmount { get; set; }

        /// <summary>
        /// 变动后余额
        /// </summary>
        [Description("变动后余额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal AfterAmount { get; set; }

        /// <summary>
        /// 商户单号
        /// </summary>
        [Description("商户单号")]
        [Column(StringLength = 64, IsNullable = false)]
        public string OutTradeNo { get; set; }

        /// <summary>
        /// 交易时间
        /// </summary>
        [Description("交易时间")]
        public DateTime RecordDate { get; set; }

        /// <summary>
        /// 交易描述
        /// </summary>
        [Description("交易描述")]
        [Column(StringLength = 100, IsNullable = false)]
        public string RecordDescription { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        [Column(StringLength = 100)]
        public string Remark { get; set; }
    }
}
