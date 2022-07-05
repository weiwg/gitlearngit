using System;
using LY.Report.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Personnel
{
    /// <summary>
    /// 员工附属部门
    /// </summary>
	[Table(Name = "T_Personnel_EmployeeOrganization")]
    [Index("idx_{tablename}_01", nameof(EmployeeId) + "," + nameof(OrganizationId), true)]
    public class PersonnelEmployeeOrganization : EntityTenantFull
    {
        /// <summary>
        /// 员工Id
        /// </summary>
		public string EmployeeId { get; set; }

        /// <summary>
        /// 员工
        /// </summary>
        public PersonnelEmployee Employee { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
		public string OrganizationId { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public PersonnelOrganization Organization { get; set; }
    }
}
