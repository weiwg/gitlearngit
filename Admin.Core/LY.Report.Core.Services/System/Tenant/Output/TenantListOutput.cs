using LY.Report.Core.Common.BaseModel.Enum;
using LY.Report.Core.Model.BaseEnum;
using Newtonsoft.Json;
using System;

namespace LY.Report.Core.Service.System.Tenant.Output
{
    public class TenantListOutput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// 授权用户
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 授权角色
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 企业编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 企业名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 数据隔离类型
        /// </summary>
        public DataIsolationType DataIsolationType { get; set; }

        /// <summary>
        /// 数据隔离类型名称
        /// </summary>
        public string DataIsolationTypeName => DataIsolationType.ToDescriptionOrString();

        /// <summary>
        /// 数据库
        /// </summary>
        [JsonIgnore]
        public FreeSql.DataType? DbType { get; set; }

        /// <summary>
        /// 数据库名称
        /// </summary>
        public string DbTypeName => DbType.ToDescriptionOrString();

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 空闲时间(分)
        /// </summary>
        public int? IdleTime { get; set; }

        /// <summary>
        /// 启用
        /// </summary>
		public IsActive IsActive { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }
    }
}
