using System.ComponentModel;

namespace LY.Report.Core.Model.Driver.Enum
{
    /// <summary>
    /// 司机状态
    /// </summary>
    public enum DriverStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")] 
        Normal = 1,

        /// <summary>
        /// 锁定
        /// </summary>
        [Description("锁定")] 
        Lock = 2,

        /// <summary>
        /// 注销
        /// </summary>
        [Description("注销")] 
        Cancellation = 3
    }
}
