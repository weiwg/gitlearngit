using System.ComponentModel;

namespace LY.Report.Core.Model.Order.Enum
{
    /// <summary>
    /// 订单类型
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// 用户发单
        /// </summary>
        [Description("用户发单")]
        User = 1,
        /// <summary>
        /// 商城发单
        /// </summary>
        [Description("商城发单")]
        Store = 2,
        /// <summary>
        /// 商城发单(指定司机)
        /// </summary>
        [Description("商城发单(指定司机)")]
        StoreDriver = 3
    }

    /// <summary>
    /// 订单状态
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// 待支付
        /// </summary>
        [Description("待支付")]
        Unpaid = 1,
        /// <summary>
        /// 待接单
        /// </summary>
        [Description("待接单")]
        Waiting = 2,
        /// <summary>
        /// 已接单
        /// </summary>
        [Description("已接单")]
        Received = 3,
        /// <summary>
        /// 送货中
        /// </summary>
        [Description("送货中")]
        Delivering = 4,
        /// <summary>
        /// 已送达
        /// </summary>
        [Description("已送达")]
        Delivered = 5,
        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Completed = 6,
        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已取消")]
        Cancelled = 7,
        /// <summary>
        /// 原路退回
        /// </summary>
        [Description("原路退回")]
        ReturnBack = 88
    }

    /// <summary>
    /// 订单取消状态
    /// </summary>
    public enum CancelStatus
    {
        /// <summary>
        /// 未取消
        /// </summary>
        [Description("未取消")]
        NotCancelled = 1,
        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已取消")]
        Cancelled = 2
    }
}
