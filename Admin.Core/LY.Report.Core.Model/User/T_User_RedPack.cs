using FreeSql.DataAnnotations;
using System;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.User.Enum;


namespace LY.Report.Core.Model.User
{
    /// <summary>
    /// 用户红包
    /// </summary>
    [Table(Name = "T_User_RedPack")]
    [Index("idx_{tablename}_01", nameof(RedPackRecordId), true)]
    public class UserRedPack : EntityTenantFull
    {
        /// <summary>
        /// 红包记录ID
        /// </summary>
        [Description("红包记录Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string RedPackRecordId { get; set; }
        /// <summary>
        /// 红包ID
        /// </summary>
        [Description("红包Id")]
        public string RedPackId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [Description("用户Id")]

        public string UserId { get; set; }
        /// <summary>
        /// 红包名称
        /// </summary>
        [Description("红包名称")]
        [Column(StringLength = 50, IsNullable = false)]
        public string RedPackName { get; set; }

        /// <summary>
        /// 红包金额
        /// </summary>
        [Description("红包金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal RedPackAmount { get; set; }
        /// <summary>
        /// 剩余金额
        /// </summary>
        [Description("剩余金额")]
        [Column(Precision = 12, Scale = 2)]
        public decimal RemainAmount { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        [Description("生效时间")]
        public DateTime EffectiveDate { get; set; }
        /// <summary>
        /// 失效时间
        /// </summary>
        [Description("失效时间")]

        public DateTime ExpiryDate { get; set; }
        /// <summary>
        /// 红包状态
        /// </summary>
        [Description("红包状态")]
        [Column(Position = -7, IsNullable = false, InsertValueSql = "1")]
        public UserRedPackStatus RedPackStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        public string Remark { get; set; }
    }
}
