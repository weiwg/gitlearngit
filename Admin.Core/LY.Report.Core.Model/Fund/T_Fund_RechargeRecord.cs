using System.ComponentModel;
using FreeSql.DataAnnotations;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Fund.Enum;

namespace LY.Report.Core.Model.Fund
{
    /// <summary>
    /// 充值记录
    /// </summary>
    [Table(Name = "T_Fund_RechargeRecord")]
    [Index("idx_{tablename}_01", nameof(RecordId), true)]
    public class FundRechargeRecord : EntityTenantFull
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        [Description("系统Id")]
        [Column(IsPrimary = true, Position = 2, StringLength = 36, IsNullable = false)]
        public string RecordId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Description("用户Id")]
        [Column(StringLength = 36, IsNullable = false)]
        public string UserId { get; set; }
        
        /// <summary>
        /// 充值状态
        /// </summary>
        [Description("充值状态")]
        public RechargeStatus RechargeStatus { get; set; }

        /// <summary>
        /// 充值单号
        /// </summary>
        [Description("充值单号")]
        [Column(StringLength = 64, IsNullable = false)]
        public string RechargeOrderNo { get; set; }

        /// <summary>
        /// 商户充值单号
        /// </summary>
        [Description("商户充值单号")]
        [Column(StringLength = 64, IsNullable = false)]
        public string RechargeOutTradeNo { get; set; }

        /// <summary>
        /// 充值金额
        /// </summary>
        [Description("充值金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal RechargeAmount { get; set; }

        /// <summary>
        /// 赠送金额
        /// </summary>
        [Description("赠送金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal GiveAmount { get; set; }

        /// <summary>
        /// 到账金额
        /// </summary>
        [Description("到账金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal ActualArrivalAmount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        [Column(StringLength = 100)]
        public string Remark { get; set; }

    }
}
