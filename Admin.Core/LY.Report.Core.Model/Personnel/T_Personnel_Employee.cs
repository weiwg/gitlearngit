using System;
using System.Collections.Generic;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Auth.Enum;
using LY.Report.Core.Model.System;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Personnel
{
    /// <summary>
    /// 员工
    /// </summary>
	[Table(Name = "T_Personnel_Employee")]
    [Index("idx_{tablename}_01", nameof(EmployeeId), true)]
    public class PersonnelEmployee : EntityFull, ITenant
    {
        /// <summary>
        /// 员工Id
        /// </summary>
        [Description("员工Id")]
        [Column(IsPrimary = true, Position = 2, StringLength = 36, IsNullable = false)]
        public string EmployeeId { get; set; }
        /// <summary>
        /// 租户Id
        /// </summary>
        [Column(Position = -10)]
        public string TenantId { get; set; }

        public SysTenant Tenant { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        //public UserEntity User { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Column(StringLength = 20)]
        public string Name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Column(StringLength = 20)]
        public string NickName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Sex? Sex { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        [Column(StringLength = 20)]
        public string Code { get; set; }

        /// <summary>
        /// 主属部门Id
        /// </summary>
        public string OrganizationId { get; set; }

        public PersonnelOrganization Organization { get; set; }

        /// <summary>
        /// 主管Id
        /// </summary>
        public string PrimaryEmployeeId { get; set; }

        /// <summary>
        /// 主管
        /// </summary>
        public PersonnelEmployee PrimaryEmployee { get; set; }

        /// <summary>
        /// 职位Id
        /// </summary>
        public string PositionId { get; set; }

        public PersonnelPosition Position { get; set; }


        /// <summary>
        /// 手机号
        /// </summary>
        [Column(StringLength = 20)]
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Column(StringLength = 250)]
        public string Email { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? HireDate { get; set; }

        [Navigate(ManyToMany = typeof(PersonnelEmployeeOrganization))]
        public ICollection<PersonnelOrganization> Organizations { get; set; }
    }
}
