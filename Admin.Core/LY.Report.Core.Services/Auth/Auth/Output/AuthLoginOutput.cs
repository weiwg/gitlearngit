using LY.Report.Core.Common.BaseModel.Enum;

namespace LY.Report.Core.Service.Auth.Auth.Output
{
    public class AuthLoginOutput
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string NickName { get; set; }

        private string _tenantId;
        /// <summary>
        /// 租户Id
        /// </summary>
        public string TenantId { get { return string.IsNullOrEmpty(_tenantId) ? "" : _tenantId; } set { _tenantId = value; } }

        /// <summary>
        /// 租户类型
        /// </summary>
        public TenantType? TenantType { get; set; }

        /// <summary>
        /// 数据隔离
        /// </summary>
        public DataIsolationType? DataIsolationType { get; set; }
    }
}
