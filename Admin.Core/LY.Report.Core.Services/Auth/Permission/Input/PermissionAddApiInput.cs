using LY.Report.Core.Model.Auth.Enum;
using System.ComponentModel.DataAnnotations;

namespace LY.Report.Core.Service.Auth.Permission.Input
{
    public class PermissionAddApiInput
    {
        /// <summary>
        /// 权限类型
        /// </summary>
        public PermissionType Type { get; set; }

        /// <summary>
        /// 父级节点
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 接口
        /// </summary>
        public string ApiId { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 隐藏
        /// </summary>
		public bool Hidden { get; set; }

        ///// <summary>
        ///// 启用
        ///// </summary>
        //public IsActive IsActive { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 接口所属版本号
        /// </summary>
        [Display(Name = "Api版本号")]
        [Required(ErrorMessage = "版本号不能为空"), StringLength(20, ErrorMessage = "{0} 限制为{2}-{1} 个字符。", MinimumLength = 2)]
        public string ApiVersion { get; set; }
    }
}
