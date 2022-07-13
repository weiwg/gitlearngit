using FreeSql.DataAnnotations;
using LY.Report.Core.Common.BaseModel;
using System;
using System.ComponentModel;

namespace LY.Report.Core.Model.Product
{
    /// <summary>
    /// 项目实体
    /// </summary>
    [Table(Name = "T_Product_ProductInfo")]
    [Index("idx_{tablename}_01", nameof(ProId), true)]
    public class ProductInfo: EntityTenantFull
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        [Description("项目ID")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string ProId { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        [Description("项目编号")]
        [Column(StringLength = 150, IsNullable = false)]
        public string ProNO { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        [Column(IsNullable = false)]
        public int Status { get; set; } = 0;
    }
}
