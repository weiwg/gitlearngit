using System.ComponentModel;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Record
{
    /// <summary>
    /// 操作日志
    /// </summary>
    [Table(Name = "T_Record_Operation", OldName = "T_Log_Operation")]
    public class RecordOperation : BaseRecord
    {
        /// <summary>
        /// 接口名称
        /// </summary>
        [Description("接口名称")]
        [Column(Position = 2, StringLength = 50)]
        public string ApiLabel { get; set; }

        /// <summary>
        /// 接口地址
        /// </summary>
        [Description("接口地址")]
        [Column(Position = 3, StringLength = 500)]
        public string ApiPath { get; set; }

        /// <summary>
        /// 接口提交方法
        /// </summary>
        [Description("接口提交方法")]
        [Column(Position = 4, StringLength = 50)]
        public string ApiMethod { get; set; }

        /// <summary>
        /// 操作参数
        /// </summary>
        [Description("操作参数")]
        [Column(StringLength = -1)]
        public string Params { get; set; }
    }
}
