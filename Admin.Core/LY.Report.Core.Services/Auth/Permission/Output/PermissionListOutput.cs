using LY.Report.Core.Model.Auth.Enum;

namespace LY.Report.Core.Service.Auth.Permission.Output
{
    public class PermissionListOutput
    {
        /// <summary>
        /// 权限Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 权限Id(自定义主键)
        /// </summary>
        public string PermissionId { get; set; }

        /// <summary>
        /// 父级节点
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string Label { get; set; }


        /// <summary>
        /// 权限类型
        /// </summary>
        public PermissionType Type { get; set; }

        /// <summary>
        /// 访问地址
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 接口路径
        /// </summary>
        public string ApiPaths { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 组打开
        /// </summary>
        public bool? Opened { get; set; }

        /// <summary>
        /// 隐藏
        /// </summary>
		public bool Hidden { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public bool IsActive { get; set; }
        /// <summary>
        /// api版本号
        /// </summary>
        public string ApiVersion { get; set; }
    }
}