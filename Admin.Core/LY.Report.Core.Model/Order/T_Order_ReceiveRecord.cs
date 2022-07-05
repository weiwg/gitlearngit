using System;
using System.ComponentModel;
using LY.Report.Core.Common.BaseModel;
using LY.Report.Core.Model.Order.Enum;
using FreeSql.DataAnnotations;

namespace LY.Report.Core.Model.Order
{
    /// <summary>
    /// 司机接单记录
    /// </summary>
    [Table(DisableSyncStructure = true, Name = "T_Order_ReceiveRecord")]
    [Index("idx_{tablename}_01", nameof(RecordId), true)]
    public class OrderReceiveRecord : EntityTenantFull
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        [Description("记录Id")]
        [Column(IsPrimary = true, StringLength = 36, IsNullable = false)]
        public string RecordId { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [Description("订单号")]
        [Column(StringLength = 32, IsNullable = false)]
        public string OrderNo { get; set; }

        /// <summary>
        /// 司机Id
        /// </summary>
        [Description("司机Id")]
        [Column(StringLength = 36)]
        public string DriverId { get; set; }

        /// <summary>
        /// 接单时间
        /// </summary>
        [Description("接单时间")]
        public DateTime ReceivedOrderDate { get; set; }

        /// <summary>
        /// 取消状态
        /// </summary>
        [Description("取消状态")]
        public CancelStatus CancelStatus { get; set; }

        /// <summary>
        /// 取消时间
        /// </summary>
        [Description("取消时间")]
        public DateTime? CancelDate { get; set; }

        /// <summary>
        /// 取消原因
        /// </summary>
        [Description("取消原因")]
        [Column(StringLength = 100)]
        public string CancelReason { get; set; }
    }
}
