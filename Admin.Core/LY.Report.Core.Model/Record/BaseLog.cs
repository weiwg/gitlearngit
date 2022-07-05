using System;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Record
{
    /// <summary>
    /// 日志基类
    /// </summary>
    public abstract class BaseRecord : EntityAdd, IEntitySoftDelete, ITenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        [Description("租户Id")]
        [Column(Position = -1, CanUpdate = false)]
        public string TenantId { get; set; }

        private string _logId;
        /// <summary>
        /// 日志Id
        /// </summary>
        [Description("日志Id")]
        [Column(IsPrimary = true, Position = 2, IsNullable = false)]
        public string LogId { get => string.IsNullOrEmpty(_logId) ? Guid.NewGuid().ToString("D") : _logId; set => _logId = value; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Description("用户名")]
        [Column(StringLength = 60)]
        public string UserName { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        [Description("IP")]
        [Column(StringLength = 100)]
        public string IP { get; set; }

        /// <summary>
        /// 浏览器
        /// </summary>
        [Description("浏览器")]
        [Column(StringLength = 100)]
        public string Browser { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        [Description("操作系统")]
        [Column(StringLength = 100)]
        public string Os { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        [Description("设备")]
        [Column(StringLength = 50)]
        public string Device { get; set; }

        /// <summary>
        /// 浏览器信息
        /// </summary>
        [Description("浏览器信息")]
        [Column(StringLength = -1)]
        public string BrowserInfo { get; set; }

        /// <summary>
        /// 耗时（毫秒）
        /// </summary>
        [Description("耗时")]
        public long ElapsedMilliseconds { get; set; }

        /// <summary>
        /// 操作状态
        /// </summary>
        [Description("操作状态")]
        public bool Status { get; set; }

        /// <summary>
        /// 操作消息
        /// </summary>
        [Description("操作消息")]
        [Column(StringLength = 500)]
        public string Msg { get; set; }

        /// <summary>
        /// 操作结果
        /// </summary>
        [Description("操作结果")]
        [Column(StringLength = -1)]
        public string Result { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        [Description("是否删除")]
        [Column(Position = -1)]
        public bool IsDel { get; set; } = false;
    }
}
