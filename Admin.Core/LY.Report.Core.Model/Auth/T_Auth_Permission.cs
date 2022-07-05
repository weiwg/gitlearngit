using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Auth.Enum;
using LY.Report.Core.Model.BaseEnum;
using FreeSql.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;

namespace LY.Report.Core.Model.Auth
{
    /// <summary>
    /// 权限
    /// </summary>
	[Table(Name = "T_Auth_Permission")]
    [Index("idx_{tablename}_01", nameof(PermissionId), true)]
    public class AuthPermission : EntityTenantFull
    {
        /// <summary>
        /// 父级节点
        /// </summary>
        public string ParentId { get; set; }

        [Navigate(nameof(ParentId))]
        public List<AuthPermission> Childs { get; set; }

        /// <summary>
        /// 权限id
        /// </summary>
        [Description("权限id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string PermissionId { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        [Column(StringLength = 50)]
        public string Label { get; set; }

        /// <summary>
        /// 权限编码
        /// </summary>
        [Column(StringLength = 550)]
        public string Code { get; set; }

        /// <summary>
        /// 权限类型
        /// </summary>
        [Column(MapType = typeof(int),CanUpdate = false)]
        public PermissionType Type { get; set; }

        private string _viewId;
        /// <summary>
        /// 视图
        /// </summary>
        public string ViewId { get =>_viewId; set => _viewId= value.IsNull() ? "" : value; }
        public AuthView View { get; set; }

        private string _apiId;
        /// <summary>
        /// 接口
        /// </summary>
        public string ApiId { get => _apiId; set => _apiId = value.IsNull() ? "" : value;}

        private string _path;
        /// <summary>
        /// 菜单访问地址
        /// </summary>
        [Column(StringLength = 500)]
        public string Path { get => _path; set => _path = value.IsNull() ? "" : value; }

        /// <summary>
        /// 图标
        /// </summary>
        [Column(StringLength = 100)]
        public string Icon { get; set; }

        /// <summary>
        /// 隐藏
        /// </summary>
		public bool Hidden { get; set; } = false;

        /// <summary>
        /// 启用
        /// </summary>
		public IsActive IsActive { get; set; } = IsActive.Yes;

        /// <summary>
        /// 可关闭
        /// </summary>
        public bool Closable { get; set; } = false;

        /// <summary>
        /// 打开组
        /// </summary>
        public bool Opened { get; set; } = false;

        /// <summary>
        /// 打开新窗口
        /// </summary>
        public bool NewWindow { get; set; } = false;

        /// <summary>
        /// 链接外显
        /// </summary>
        public bool External { get; set; } = false;

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } = 0;

        /// <summary>
        /// 描述
        /// </summary>
        [Column(StringLength = 100)]
        public string Description { get; set; }

        [Navigate(ManyToMany = typeof(AuthPermissionApi))]
        public ICollection<AuthApi> Apis { get; set; }
        /// <summary>
        /// Api版本号
        /// </summary>
        [Description("Api版本号")]
        [Column(IsNullable = false,StringLength =100)]
        public string ApiVersion { get; set; } = "V0";
    }
}
