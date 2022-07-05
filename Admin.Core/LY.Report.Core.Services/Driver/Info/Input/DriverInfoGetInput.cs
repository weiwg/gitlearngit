using LY.Report.Core.Model.Driver.Enum;

namespace LY.Report.Core.Service.Driver.Info.Input
{
    /// <summary>
    /// 查询
    /// </summary>
    public class DriverInfoGetInput
    {
        /// <summary>
        /// 司机Id
        /// </summary>
        public string DriverId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 司机姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 司机类型
        /// </summary>
        public DriverType DriverType { get; set; }

        /// <summary>
        /// 司机状态
        /// </summary>
        public DriverStatus DriverStatus { get; set; }
    }
}
