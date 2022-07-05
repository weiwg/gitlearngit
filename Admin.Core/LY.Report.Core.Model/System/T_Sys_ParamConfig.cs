using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.BaseEnum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.System
{
    /// <summary>
    /// 系统参数配置表
    /// </summary>
    [Table(Name = "T_Sys_ParamConfig")]
    [Index("Idx_{tablename}_01", nameof(ParamKey), true)]
    public class SysParamConfig : EntityTenantFull
    {
        /// <summary>
        /// 设置Id
        /// </summary>
        [Description("设置Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string ConfigId { get; set; }

        /// <summary>
        /// 参数Key
        /// </summary>
        [Description("参数Key")]
        [Column(IsPrimary = true, StringLength = 50, IsNullable = false)]
        public string ParamKey { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        [Description("参数名称")]
        [Column(StringLength = 50, IsNullable = false)]
        public string ParamName { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        [Description("参数值")]
        [Column(StringLength = 50, IsNullable = false)]
        public string ParamValue { get; set; }
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
