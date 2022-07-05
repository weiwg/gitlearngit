using System.ComponentModel;
using FreeSql.DataAnnotations;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Pay.Enum;

namespace LY.Report.Core.Model.Fund
{
    /// <summary>
    /// 资金账号
    /// </summary>
    [Table(Name = "T_Fund_AccountInfo")]
    [Index("idx_{tablename}_01", nameof(AccountId), true)]
    public class FundAccountInfo : EntityTenantFull
    {
        /// <summary>
        /// 账号Id
        /// </summary>
        [Description("账号Id")]
        [Column(IsPrimary = true, Position = 2, StringLength = 36, IsNullable = false)]
        public string AccountId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Description("用户Id")]
        [Column(StringLength = 36, IsNullable = false)]
        public string UserId { get; set; }

        /// <summary>
        /// 资金平台
        /// </summary>
        [Description("资金平台")]
        public FundPlatform FundPlatform { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Description("账号")]
        [Column(StringLength = 100, IsNullable = false)]
        public string AccountNo { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Description("姓名")]
        [Column(StringLength = 50, IsNullable = false)]
        public string AccountName { get; set; }

        /// <summary>
        /// 开户行
        /// </summary>
        [Description("开户行")]
        [Column(StringLength = 100, IsNullable = false)]
        public string BankName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        [Column(StringLength = 100)]
        public string Remark { get; set; }

    }
}
