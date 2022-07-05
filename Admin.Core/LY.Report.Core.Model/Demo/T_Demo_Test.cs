using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.BaseEnum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Demo
{
    /// <summary>
    /// Test模板
    /// </summary>
    [Table(DisableSyncStructure = true, Name = "T_Demo_Test")]
    [Index("idx_{tablename}_01", nameof(TestId), true)]
    public class DemoTest : EntityTenantFull
    {
        /// <summary>
        /// TestId
        /// </summary>
        [Description("TestId")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string TestId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        [Column(StringLength = 20, IsNullable = false)]
        public string TestName { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片")]
        [Column(StringLength = 100)]
        public string TestImg { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Description("数量")]
        public int TestCount { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [Description("价格")]
        [Column(Precision = 12, Scale = 2)]
        public decimal TestPrice { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        [Description("是否有效")]
        [Column(IsNullable = false)]
        public IsActive IsActive { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Description("备注")]
        [Column(StringLength = 100)]
        public string Remark { get; set; }
    }
}