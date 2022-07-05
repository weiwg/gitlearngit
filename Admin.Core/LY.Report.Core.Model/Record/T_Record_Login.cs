using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Record
{
    /// <summary>
    /// 操作日志
    /// </summary>
	[Table(Name = "T_Record_Login", OldName = "T_Log_Login")]
    public class RecordLogin : BaseRecord
    {
    }
}
