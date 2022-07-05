using System;
using System.Collections.Generic;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.BaseEnum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Personnel
{
    /// <summary>
    /// 组织机构
    /// </summary>
	[Table(Name = "T_Personnel_Organization")]
    [Index("idx_{tablename}_01", nameof(OrgId), true)]
    public class PersonnelOrganization : EntityFull, ITenant
    {
        /// <summary>
        /// 组织Id
        /// </summary>
        [Description("组织Id")]
        [Column(IsPrimary = true, Position = 2, StringLength = 36, IsNullable = false)]
        public string OrgId { get; set; }
        /// <summary>
        /// 租户Id
        /// </summary>
        [Column(Position = -10, CanUpdate = false)]
        public string TenantId { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
		public string ParentId { get; set; }

        [Navigate(nameof(ParentId))]
        public List<PersonnelEmployeeOrganization> Childs { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Column(StringLength = 50)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Column(StringLength = 50)]
        public string Code { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [Column(StringLength = 50)]
        public string Value { get; set; }

        /// <summary>
        /// 主管Id
        /// </summary>
        public string PrimaryEmployeeId { get; set; }

        /// <summary>
        /// 主管
        /// </summary>
        public PersonnelEmployee PrimaryEmployee { get; set; }

        /// <summary>
        /// 员工人数
        /// </summary>
        public int EmployeeCount { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Column(StringLength = 500)]
        public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public IsActive IsActive { get; set; } = IsActive.Yes;

        /// <summary>
        /// 排序
        /// </summary>
		public int Sort { get; set; }

        [Navigate(ManyToMany = typeof(PersonnelEmployeeOrganization))]
        public ICollection<PersonnelEmployee> Employees { get; set; }
    }
}
