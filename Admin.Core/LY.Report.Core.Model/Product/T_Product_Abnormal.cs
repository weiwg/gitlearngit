using FreeSql.DataAnnotations;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Product.Enum;
using System;
using System.ComponentModel;

namespace LY.Report.Core.Model.Product
{
    /// <summary>
    /// 异常实体
    /// </summary>
    [Table(Name = "T_Product_Abnormal")]
    [Index("idx_{tablename}_01", nameof(AbnormalNo), true)]
    public class Abnormal: EntityTenantFull
    {
        /// <summary>
        /// 异常单号
        /// </summary>
        [Description("异常单号")]
        [Column(IsNullable = false, IsPrimary = true, StringLength = 14)]
        public string AbnormalNo { get; set; }
        /// <summary>
        /// 项目编号
        /// </summary>
        [Description("项目编号")]
        [Column(StringLength = 150, IsNullable = false)]
        public string ProjectNo { get; set; }
        /// <summary>
        /// 线体
        /// </summary>
        [Description("线体")]
        [Column(StringLength = 150, IsNullable = false)]
        public string LineName { get; set; }
        /// <summary>
        /// 班别
        /// </summary>
        [Description("班别")]
        [Column(StringLength = 150, IsNullable = false)]
        public string ClassAB { get; set; }
        /// <summary>
        /// 站点
        /// </summary>
        [Description("站点")]
        [Column(StringLength = 150, IsNullable = false)]
        public string FProcess { get; set; }
        /// <summary>
        /// 大类(异常/停线)
        /// </summary>
        [Column(IsNullable = false)]
        public AbnormalType Type { get; set; }
        /// <summary>
        /// 小类(机器故障/物料异常/停电等)
        /// </summary>
        [Column(IsNullable = false)]
        public AbnormalItemType ItemType { get; set; }
        /// <summary>
        /// 异常描述
        /// </summary>
        [Description("异常描述")]
        [Column(StringLength = 500)]
        public string Description { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        [Description("开始时间")]
        [Column(CanUpdate = false, ServerTime = DateTimeKind.Local, IsNullable = false)]
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [Description("结束时间")]
        [Column(CanUpdate = false, ServerTime = DateTimeKind.Local)]
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 责任部门
        /// </summary>
        [Description("责任部门")]
        [Column(IsNullable = false)]
        public ResponDepart ResponDepart { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        [Description("责任人")]
        [Column(StringLength = 36, IsNullable = false)]
        public string ResponBy { get; set; }
        /// <summary>
        /// 原因分析
        /// </summary>
        [Description("原因分析")]
        [Column(StringLength = 500)]
        public string Reason { get; set; }
        /// <summary>
        /// 临时对策
        /// </summary>
        [Description("临时对策")]
        [Column(StringLength = 500)]
        public string TempMeasures { get; set; }
        /// <summary>
        /// 根本对策
        /// </summary>
        [Description("根本对策")]
        [Column(StringLength = 500)]
        public string FundaMeasures { get; set; }
        /// <summary>
        /// 异常状态
        /// </summary>
        [Description("异常状态")]
        [Column(IsNullable = false)]
        public AbnormalStatus Status { get; set; } = AbnormalStatus.Unhandled;
    }
}
