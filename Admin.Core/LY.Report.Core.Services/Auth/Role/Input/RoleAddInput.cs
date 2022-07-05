using LY.Report.Core.Model.Auth.Enum;
using LY.Report.Core.Model.BaseEnum;
using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Auth.Role.Input
{
    /// <summary>
    /// 添加
    /// </summary>
    public class RoleAddInput
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        //[Display(Name = "角色名称")]
        //[Required(ErrorMessage = "角色名称不能为空"), StringLength(20, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public RoleType RoleType { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public IsActive IsActive { get; set; }

        /// <summary>
        /// 创建用户Id
        /// </summary>
        public string CreateUserId { get; set; }
    }
}
