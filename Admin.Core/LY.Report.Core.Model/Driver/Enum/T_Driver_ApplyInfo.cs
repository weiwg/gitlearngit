using System.ComponentModel;

namespace LY.Report.Core.Model.Driver.Enum
{
    /// <summary>
    /// 申请类型
    /// </summary>
    public enum ApplyType
    {
        /// <summary>
        /// 申请司机
        /// </summary>
        [Description("申请司机")]
        Add = 1,
        /// <summary>
        /// 修改信息
        /// </summary>
        [Description("修改信息")]
        Update = 2
    }

    /// <summary>
    /// 司机类型
    /// </summary>
    public enum DriverType
    {
        /// <summary>
        /// 所有
        /// </summary>
        [Description("所有")]
        All = 0,
        /// <summary>
        /// 入驻
        /// </summary>
        [Description("入驻")]
        Join = 1,
        /// <summary>
        /// 商家
        /// </summary>
        [Description("商家")]
        Store = 2
    }

    /// <summary>
    /// 审核状态
    /// </summary>
    public enum ApprovalStatus
    {
        /// <summary>
        /// 申请中
        /// </summary>
        [Description("申请中")]
        Applying = 1,
        /// <summary>
        /// 通过
        /// </summary>
        [Description("通过")]
        Approval = 2,
        /// <summary>
        /// 未通过
        /// </summary>
        [Description("未通过")]
        UnApproval = 3
    }
}
