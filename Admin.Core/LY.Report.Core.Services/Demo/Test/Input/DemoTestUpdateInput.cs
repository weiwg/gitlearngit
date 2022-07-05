using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Demo.Test.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public partial class DemoTestUpdateInput : DemoTestAddInput
    {
        /// <summary>
        /// TestId
        /// </summary>
        public string TestId { get; set; }

    }
}
