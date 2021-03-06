using LY.Report.Core.Model.BaseEnum;

namespace LY.Report.Core.Service.Personnel.Position.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class PositionAddInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public IsActive IsActive { get; set; }
    }
}
