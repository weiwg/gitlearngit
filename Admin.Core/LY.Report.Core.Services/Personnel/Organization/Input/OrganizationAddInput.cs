using LY.Report.Core.Model.BaseEnum;

namespace LY.Report.Core.Service.Personnel.Organization.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class OrganizationAddInput
    {
        /// <summary>
        /// 父级
        /// </summary>
		public string ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 主管
        /// </summary>
        public string PrimaryUserId { get; set; }

        /// <summary>
        /// 员工人数
        /// </summary>
        public int EmployeeCount { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public IsActive IsActive { get; set; }
    }
}
