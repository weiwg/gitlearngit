

using LY.Report.Core.Model.BaseEnum;

namespace LY.Report.Core.Service.Demo.Test.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class DemoTestGetInput
    {
        /// <summary>
        /// TestId
        /// </summary>
        public string TestId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public IsActive IsActive { get; set; }
    }
}
