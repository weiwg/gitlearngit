using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.System
{
    /// <summary>
    /// 系统参数配置表
    /// </summary>
    [Table(Name = "T_Sys_WebConfig")]
    [Index("Idx_{tablename}_01", nameof(ConfigId), true)]
    public class SysWebConfig : EntityTenantFull
    {
        /// <summary>
        /// 设置Id
        /// </summary>
        [Description("设置Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string ConfigId { get; set; }
    }
}
