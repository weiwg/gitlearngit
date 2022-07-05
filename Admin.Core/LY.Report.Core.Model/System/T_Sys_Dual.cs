using FreeSql.DataAnnotations;
using System;
using LY.Report.Core.Common.BaseModel;

namespace LY.Report.Core.Model.Auth
{
    /// <summary>
    /// 系统表，用于查询系统函数
    /// </summary>
    [Table(Name = "T_Sys_Dual")]
    public class T_Sys_Dual: EntityTenantFull
    {
    }  
}
