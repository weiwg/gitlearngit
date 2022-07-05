using LY.Report.Core.Model.Auth.Enum;
using System;

namespace LY.Report.Core.Service.Auth.UserRole.Output
{
    public class UserRoleGetOutput
    {
        /// <summary>
        /// Id(普通字段)
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// UserRoleId(主键)
        /// </summary>
        public string UserRoleId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色类型
        /// </summary>
        public RoleType RoleType { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        public string CreateUserId { get; set; }

        /// <summary>
        /// 更新人id
        /// </summary>
        public string UpdateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
