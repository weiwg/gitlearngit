using FreeSql.DataAnnotations;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Product.Enum;
using System;
using System.ComponentModel;

namespace LY.Report.Core.Model.Product
{
    /// <summary>
    /// 责任人实体
    /// </summary>
    [Table(Name = "T_Product_AbnormalPerson")]
    [Index("idx_{tablename}_01", nameof(PersonLiableId), true)]
    public class AbnormalPerson: EntityTenantFull
    {
        /// <summary>
        /// 责任人Id
        /// </summary>
        [Description("责任人Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string PersonLiableId { get; set; }
        /// <summary>
        /// 英文名字
        /// </summary>
        [Description("英文名字")]
        [Column(StringLength = 150, IsNullable = false)]
        public string EnName { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        [Description("名字")]
        [Column(StringLength = 150, IsNullable = false)]
        public string Name { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        [Description("职位")]
        [Column(StringLength = 150)]
        public string Position { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        [Description("部门")]
        [Column(StringLength = 150)]
        public ResponDepart Department { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Description("手机号")]
        [Column(StringLength = 150)]
        public string Phone { get; set; }

        /// <summary>
        /// NPI小手机号
        /// </summary>
        [Description("NPI小手机号")]
        [Column(StringLength = 150)]
        public string DumpPhone { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [Description("工号")]
        [Column(StringLength = 150, IsNullable = false)]
        public string JobNo { get; set; }
    }
}
