using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.Report.Core.Service.Auth.UserRole.Input
{
    /// <summary>
    /// 修改
    /// </summary>
    public class UserRoleUpdateInput:UserRoleAddInput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public string UserRoleId { get; set; }

        /// <summary>
        /// 更新用户角色人id
        /// </summary>
        public string UpdateUserId { get; set; }
    }
}
